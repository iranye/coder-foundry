using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class Notifications
    {
        public int Id { get; set; }
        public List<TicketNotification> TicketNotifications { get; set; }

        public int NotificationsCount => TicketNotifications.Count;
    }

    public class AjaxController : Controller
    {
        public JsonResult GetUnreadNotificationCount()
        {
            var unreadNotificationsCount = NotificationHelper.GetUnreadNotificationsCount();
            return Json(unreadNotificationsCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTicketTypes()
        {
            var ticketTypesBarChart = new TicketTypes();
            var viewModel = new MainDashboardViewModel();
            List<Ticket> allTickets = viewModel.AllTickets;

            if (allTickets.Count > 0)
            {
                Dictionary<string, int> ticketStatusCounts = new Dictionary<string, int>();
                foreach (var status in ticketTypesBarChart.labels)
                {
                    ticketStatusCounts[status] = 0;
                }
                foreach (var ticket in allTickets)
                {
                    ticketStatusCounts[ticket.TicketStatus.Name]++;
                }

                for (int i = 0; i < ticketTypesBarChart.labels.Length; i++)
                {
                    int percentage = (100 * ticketStatusCounts[ticketTypesBarChart.labels[i]]) / allTickets.Count;
                    ticketTypesBarChart.values[i] = percentage;
                }

                while (ticketTypesBarChart.values.Sum(s => s) < 100)
                {
                    for (int i = 0; i < ticketTypesBarChart.values.Length; i++)
                    {
                        if (ticketTypesBarChart.values[i] <  2)
                        {
                            ticketTypesBarChart.values[i]++;
                        }
                    }
                }
            }
            return Json(ticketTypesBarChart);
        }
    }

    public class TicketTypes
    {
        public string[] labels = new[] { "Open", "Assigned", "On Hold", "In Progress", "Needs Remediation", "Pending Approval", "Resolved" };
        public int[] values = new[] { 20, 28, 22, 10, 10, 10, 10 };
    }
}