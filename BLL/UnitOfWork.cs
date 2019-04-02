﻿using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UnitOfWork
    {
        public SozlukContext db;

        public UnitOfWork()
        {
            // double lock pattern
            // thread safe olarak db'nin tek bir kez üretilmesini sağlamak
            object oylesine = "";
            if (db == null)
            {
                lock (oylesine)
                {
                    if (db == null)
                    {
                        db = new SozlukContext();
                    }
                }
            }

            Languages = new BaseRepository<Language>(db);
            Words = new WordRepository(db);
            WordRequests = new BaseRepository<WordRequest>(db);
            TranslateManager = new TranslateManager(db);
        }

        public static SozlukContext Create()  //Identity static olan context döndüren bir metod istiyo
        {
            return new SozlukContext();
        }

        public bool Complete()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public BaseRepository<Language> Languages;
        public WordRepository Words;
        public BaseRepository<WordRequest> WordRequests;
        public TranslateManager TranslateManager;

    }
}
