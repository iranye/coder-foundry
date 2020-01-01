using System;
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
                    float percentage = (float) 100 * ticketStatusCounts[ticketTypesBarChart.labels[i]] / allTickets.Count;
                    percentage = (float) Math.Round(percentage);
                    ticketTypesBarChart.values[i] = (int) percentage;
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