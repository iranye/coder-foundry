using IrasBlog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IrasBlog.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IrasBlog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IrasBlog.Models.ApplicationDbContext context)
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityResult adminRole = null;
            IdentityResult moderatorRole = null;

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                adminRole = roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                moderatorRole = roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            string emailAdmin = "admin@domain.com";
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == emailAdmin))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    FirstName = "Admin",
                    LastName = "User",
                    DisplayName = "Admin"
                }, "-ABCabc123");

            }

            var adminId = userManager.FindByEmail(emailAdmin).Id;
            userManager.AddToRole(adminId, "Admin");
        }
    }
}
