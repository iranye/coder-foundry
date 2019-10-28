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

            string adminEmail = "admin@domain.com";
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Nye",
                    DisplayName = "Admin Nye"
                }, "Password-1");
            }

            var adminId = userManager.FindByEmail(adminEmail).Id;
            userManager.AddToRole(adminId, "Admin");

            string projectManagerEmail = "pm@coderfoundry.com";
            if (!context.Users.Any(u => u.Email == projectManagerEmail))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = projectManagerEmail,
                    Email = projectManagerEmail,
                    FirstName = "Project",
                    LastName = "Manager",
                    DisplayName = "Project Manager"
                }, "Password-1");
            }

            CreateUser(context, userManager, "Demo", "Admin", "Admin", isDemo: true);
            CreateUser(context, userManager, "Demo", "ProjectManager", "ProjectManager", isDemo: true);
            CreateUser(context, userManager, "Demo", "Developer", "Developer", isDemo: true);
            CreateUser(context, userManager, "Demo", "Submitter", "Submitter", isDemo: true);

            var projectManagerId = userManager.FindByEmail(projectManagerEmail).Id;
            userManager.AddToRole(projectManagerId, "ProjectManager");

            string devEmail = "dev@coderfoundry.com";
            if (!context.Users.Any(u => u.Email == devEmail))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = devEmail,
                    Email = devEmail,
                    FirstName = "Dev",
                    LastName = "Guy",
                    DisplayName = "Dev Guy"
                }, "Password-1");
            }

            var devId = userManager.FindByEmail(devEmail).Id;
            userManager.AddToRole(devId, "Developer");

            string submitterEmail = "submitter@coderfoundry.com";
            if (!context.Users.Any(u => u.Email == submitterEmail))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = submitterEmail,
                    Email = submitterEmail,
                    FirstName = "Sally",
                    LastName = "Submitter",
                    DisplayName = "Sally Submitter"
                }, "Password-1");
            }

            var submitterId = userManager.FindByEmail(submitterEmail).Id;
            userManager.AddToRole(submitterId, "Submitter");

            //var projects = context.Projects.ToList();
            //if (projects.Count == 0)
            //{
            //    return;
            //}
            //context.Tickets.AddOrUpdate(
            //    t => t.Title,
            //    new Ticket{ProjectId = projects.FirstOrDefault(p => !String.IsNullOrWhiteSpace(p.Name)).Id});
        }

        private void CreateUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, string firstName, string lastName, string role, bool isDemo = false)
        {
            var password = isDemo ? WebConfigurationManager.AppSettings["DemoPassword"] : WebConfigurationManager.AppSettings["DefaultPassword"];
            string mailinatorEmail = $"{firstName}.{lastName}@mailinator.com";
            if (!context.Users.Any(u => u.Email == mailinatorEmail))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = mailinatorEmail,
                    Email = mailinatorEmail,
                    FirstName = firstName,
                    LastName = lastName,
                    DisplayName = $"{firstName} {lastName}"
                }, password);
            }

            var userId = userManager.FindByEmail(mailinatorEmail).Id;
            userManager.AddToRole(userId, role);
        }
    }
}
