namespace DAL.Migrations
{
    using Entity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.SozlukContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DAL.SozlukContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Users.Any(x => x.Email == "admin@a.com"))
            {
                Person p1 = new Person();
                p1.Email = "admin@a.com";
                p1.UserName = "admin@a.com";

                IdentityRole role = new IdentityRole(); // yönetim kýsmý için  
                role.Name = "Administrator";
                context.Roles.Add(role);
                context.SaveChanges();

                UserStore<Person> employeeStore = new UserStore<Person>(context);
                UserManager<Person> employeeManager = new UserManager<Person>(employeeStore);

                employeeManager.Create(p1, "Aa123456!"); //kullanýcýnýn yönetici olduðunu belirtiyoruz.
                employeeManager.AddToRole(p1.Id, "Administrator");

                context.SaveChanges();
            }
        }
    }
}
