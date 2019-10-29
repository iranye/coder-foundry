using System.Web.Configuration;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityResult adminRole = null;
            IdentityResult projectManagerRole = null;
            IdentityResult developerRole = null;
            IdentityResult submitterRole = null;

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                adminRole = roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                projectManagerRole = roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                developerRole = roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                submitterRole = roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Roles.Any(r => r.Name == "DemoAdmin"))
            {
                adminRole = roleManager.Create(new IdentityRole { Name = "DemoAdmin" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoProjectManager"))
            {
                projectManagerRole = roleManager.Create(new IdentityRole { Name = "DemoProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoDeveloper"))
            {
                developerRole = roleManager.Create(new IdentityRole { Name = "DemoDeveloper" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoSubmitter"))
            {
                submitterRole = roleManager.Create(new IdentityRole { Name = "DemoSubmitter" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            CreateUser(context, userManager, "Admin", "Nye", "Admin", "admin@domain.com");
            CreateUser(context, userManager, "SuperUser", "Steve", "Admin", "admin@mailinator.com");
            CreateUser(context, userManager, "Peter", "PM", "ProjectManager", "pm@coderfoundry.com");
            CreateUser(context, userManager, "Peter", "PM", "Admin", "pm@coderfoundry.com");
            CreateUser(context, userManager, "Dave", "Developer", "Developer", "dev@coderfoundry.com", isDemo: false);
            CreateUser(context, userManager, "Sally", "Submitter", "Submitter", isDemo: false);

            CreateUser(context, userManager, "Demo", "Admin", "DemoAdmin", isDemo: true);
            CreateUser(context, userManager, "Demo", "ProjectManager", "DemoProjectManager", isDemo: true);
            CreateUser(context, userManager, "Demo", "Developer", "DemoDeveloper", isDemo: true);
            CreateUser(context, userManager, "Demo", "Submitter", "DemoSubmitter", isDemo: true);
        }

        private void CreateUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, string firstName, string lastName, string role, string email=null, bool isDemo = false)
        {
            var password = isDemo ? WebConfigurationManager.AppSettings["DemoPassword"] : WebConfigurationManager.AppSettings["DefaultPassword"];
            if (String.IsNullOrWhiteSpace(email))
            {
                email = $"{firstName}.{lastName}@mailinator.com";
            }
            if (!context.Users.Any(u => u.Email == email))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    DisplayName = $"{firstName} {lastName}"
                }, password);
            }

            var userId = userManager.FindByEmail(email).Id;
            userManager.AddToRole(userId, role);
        }
    }
}
