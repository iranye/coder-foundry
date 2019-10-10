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

            string adminEmail = "admin@domain.com";
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    DisplayName = "Admin"
                }, "-ABCabc123");

            }

            var adminId = userManager.FindByEmail(adminEmail).Id;
            userManager.AddToRole(adminId, "Admin");

            string moderatorEmail = "moderator@coderfoundry.com";
            if (!context.Users.Any(u => u.Email == moderatorEmail))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = moderatorEmail,
                    Email = moderatorEmail,
                    FirstName = "CoderFoundry",
                    LastName = "Moderator",
                    DisplayName = "CF Moderator"
                }, "Password-1");
            }

            var moderatorId = userManager.FindByEmail(moderatorEmail).Id;
            userManager.AddToRole(moderatorId, "Moderator");
        }
    }
}
