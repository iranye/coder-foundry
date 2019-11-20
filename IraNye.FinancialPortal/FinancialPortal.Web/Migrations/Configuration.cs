using System.Web.Configuration;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialPortal.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FinancialPortal.Web.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FinancialPortal.Web.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var adminRole = roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "HeadOfHousehold"))
            {
                var headOfHouseholdRole = roleManager.Create(new IdentityRole { Name = "HeadOfHousehold" });
            }
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                var memberRole = roleManager.Create(new IdentityRole { Name = "Member" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            CreateUser(context, userManager, "SuperUser", "Ira", "Admin", "admin@mailinator.com");
        }

        private void CreateUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, string firstName, string lastName, string role, string email = null)
        {
            var password = WebConfigurationManager.AppSettings["DefaultPassword"];
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
