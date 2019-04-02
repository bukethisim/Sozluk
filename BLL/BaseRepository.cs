using DAL;
using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL  //Business Logic Layer -->class library ekledik --> nuget pack dan asp.net identity yükle
{
    public class BaseRepository<T> where T : class, IEntity
        // T nin referans tipli olduğunu belirtmemiz gerekir. Bundan dolayı where T : class yazdık . Yani T'nin class olduğunu belirttik. 
        //T nin ıd sini alabilmek için Tkey kullanırız. <t,TKey> gibi kullanırsak
    {
        private SozlukContext _db; //db'nin biryerden gelmesi için

        public BaseRepository(SozlukContext db)
        {
            _db = db;
        }

        public List<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        public List<T> Search(Func<T, bool> query)
        {
            return _db.Set<T>().Where(query).ToList();
        }

        public IQueryable<T> Queryable()
        {
            return _db.Set<T>().AsQueryable();
        }

        public T GetOne(int id)
        {
            return _db.Set<T>().Find(id);
        }

        public bool Add(T record)
        {
            try
            {
                _db.Set<T>().Add(record);
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual bool Delete(int id)
        {
            try
            {
                T t = GetOne(id);
                _db.Set<T>().Remove(t);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(IEntity newRecord)
        {
            try
            {
                T old = GetOne(newRecord.Id);
                _db.Entry(old).CurrentValues.SetValues(newRecord);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
