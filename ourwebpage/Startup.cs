using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using ourwebpage.Models;

[assembly: OwinStartup(typeof(ourwebpage.Startup))]
namespace ourwebpage
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            PopulateUserAndRoles();
        }

        public void PopulateUserAndRoles()
        {
            var db = new ApplicationDbContext();

            // Identity rollerini kontrol et, yoksa oluştur
            if (!db.Set<IdentityRole>().Any(x => x.Name == MyConstants.RoleAdmin))
            {
                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                roleManager.Create(new IdentityRole(MyConstants.RoleAdmin));
            }

            if (!db.Set<IdentityRole>().Any(x => x.Name == MyConstants.RoleUser))
            {
                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                roleManager.Create(new IdentityRole(MyConstants.RoleUser));
            }

            // varsayılan Admin ve User hesaplarını Identity tablosunda oluştur
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!db.Users.Any(x => x.UserName == "appadmin"))
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "appadmin",
                    Email = "admin@spacehomy.com",
                    FullName = "SpaceHomy Admin"
                };
                var result = userManager.Create(adminUser, "123456");
                if (result.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, MyConstants.RoleAdmin);
                }
            }

            if (!db.Users.Any(x => x.UserName == "appuser"))
            {
                var normalUser = new ApplicationUser
                {
                    UserName = "appuser",
                    Email = "user@spacehomy.com",
                    FullName = "SpaceHomy User"
                };
                var result = userManager.Create(normalUser, "123456");
                if (result.Succeeded)
                {
                    userManager.AddToRole(normalUser.Id, MyConstants.RoleUser);
                }
            }

            // Mevcut Role tablosu ile senkronizasyon (Veri eksikliği olmaması için)
            if (!db.Roles.Any(r => r.Email == "admin@spacehomy.com"))
            {
                db.Roles.Add(new Role
                {
                    Name = "SpaceHomy Admin",
                    Email = "admin@spacehomy.com",
                    Password = "IdentityManagedUser",
                    RoleName = MyConstants.RoleAdmin
                });
            }

            if (!db.Roles.Any(r => r.Email == "user@spacehomy.com"))
            {
                db.Roles.Add(new Role
                {
                    Name = "SpaceHomy User",
                    Email = "user@spacehomy.com",
                    Password = "IdentityManagedUser",
                    RoleName = MyConstants.RoleUser
                });
            }

            db.SaveChanges();
        }
    }
}