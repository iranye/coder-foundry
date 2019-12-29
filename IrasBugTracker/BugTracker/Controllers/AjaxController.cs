using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;

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

            return Json(ticketTypesBarChart);
        }
    }

    public class TicketTypes
    {
        public string[] labels = new[] { "Open", "Assigned", "On Hold", "In Progress", "Needs Remediation", "Pending Approval", "Resolved" };
        public int[] values = new[] { 20, 28, 22, 10, 10, 10, 10 };
    }
}