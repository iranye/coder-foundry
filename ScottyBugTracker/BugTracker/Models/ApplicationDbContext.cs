using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistorys { get; set; }
        public DbSet<TicketNotification> TicketNotifications { get; set; }
        public DbSet<TicketPriority> TicketPrioritys { get; set; }
        public DbSet<TicketStatus> TicketStatuss { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}