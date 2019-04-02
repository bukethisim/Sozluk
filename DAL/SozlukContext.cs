using Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL //class library olarak ekliyoruz.DATA ACCESS LAYER
{
    //bu projeyede asp.net identity ekledik

    //DBContext: kullanıcı işlemleri olmayan düz db
    //IdentityDbContext<T> :T tipinde kullanıcıya göre 5 tablo ekler
    //identitydbcontext'ten miras alıyruz çünkü ordada tablolarımız var (user giriş için olan tablolar)

    public class SozlukContext : IdentityDbContext<Person>
    {
        public SozlukContext() : base("SozlukContext")
        {
            
        }
        //public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<WordRequest> WordRequests { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Table Configuration

            modelBuilder.Entity<Language>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Word>()
               .HasKey(x => x.Id);

            modelBuilder.Entity<WordRequest>()
               .HasKey(x => x.Id);

            #endregion

            #region Relations

            modelBuilder.Entity<Language>()  //1-çok ilişki 
                .HasMany(x => x.Words)
                .WithRequired(x => x.Language)
                .HasForeignKey(x=> x.Language_Id); //foreign key lazım olursa

            modelBuilder.Entity<Word>()   //kendi içinde çoka çok tablo
                .HasMany(x => x.Translations)
                .WithMany();

            modelBuilder.Entity<WordRequest>()
                .HasRequired(x => x.Person)
                .WithMany(x => x.Requests);

            modelBuilder.Entity<WordRequest>()
                .HasRequired(x => x.Language)
                .WithMany(x => x.Requests)
                .HasForeignKey(x => x.LanguageId);

            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
