using BLL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSozluk.Controllers
{
    [Authorize(Roles = "Administrator")]  //sadece adminlerin kullanabilmesi için
    public class WordsController : Controller
    {
        UnitOfWork _uw = new UnitOfWork();
        // GET: Words
        public ActionResult Index(int? langid, int? sil)
        {
            if (sil.HasValue)
            {
                _uw.Words.Delete(sil.Value);
                _uw.Complete();
                return RedirectToAction("Index", new { @langid = langid });
            }
            //ViewData,TempData obj cinsinden ondan viewde kullanırken cast yaparız
            List<SelectListItem> langs = _uw.Languages
                   .GetAll()
                   .Select(x => new SelectListItem
                   {
                       Text = x.Name,
                       Value = x.Id.ToString()
                   }).ToList();

            langs.Insert(0, new SelectListItem()
            {
                Text = "Seçiniz",
                Value = ""
            });

            ViewData["LangOptions"] = langs;

            if (langid.HasValue)
            {
                ViewBag.langid = langid.Value;
                var list = _uw.Words.Search(x => x.Language_Id == langid.Value);
                return View(list);
            }
            else
                return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            //Controllerdan View'e veri yollarken kullandıklarımız
            //ViewBag.LangOptions = _uw.Languages.GetAll(); 
            //ViewData["LangOptions"] = _uw.Languages.GetAll();
            TempData["LangOptions"] = _uw.Languages    //Ayrıca Actiondan Actiona da veri gönderebiliyo(TempData)
                .GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            return View();
        }

        [HttpPost] //ya formla iş yapıcaz yada ajaxla gönderirken post olucak
        public ActionResult Create(string WordTxt, int langid)
        {
            if (string.IsNullOrEmpty(WordTxt))
                ModelState.AddModelError("", "Kelime boş bırakılamaz.");  //Hataları daha rahat göstermek için

            if (ModelState.IsValid)
            {
                Word w = new Word();
                w.WordTxt = WordTxt;
                w.Language_Id = langid;

                var langs = _uw.Languages.GetAll();

                w.Translations = new List<Word>();
                foreach (var item in langs)
                {
                    string input = Request.Form["ceviri" + item.Id];

                    string[] words = input.Split(',');

                    foreach (string a in words)
                    {
                        if (!string.IsNullOrEmpty(a))
                        {
                            Word ceviri = new Word();
                            ceviri.Language_Id = item.Id;
                            ceviri.WordTxt = a;
                            w.Translations.Add(ceviri);
                        }
                    }

                }
                _uw.Words.Add(w);
                _uw.Complete();
                return RedirectToAction("Index");
            }
            return View();
        }
        public JsonResult AutoComplete(int langid, string q)
        {
            var list = _uw.Words
                .Search(x => x.Language_Id == langid && x.WordTxt.ToLower().StartsWith(q.ToLower()))
                .Select(x => x.WordTxt)
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}