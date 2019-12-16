using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Web.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetDisplayName(this IPrincipal principal)
        {
            var userId = principal.Identity.GetUserId();
            var currentUser = HelperMethods.GetApplicationUserByUserId(userId);
            var displayName = currentUser == null ? "UNKNOWN" : currentUser.DisplayName;

            return displayName;
        }

        public static string GetAvatarPath(this IPrincipal principal)
        {
            var userId = principal.Identity.GetUserId();
            return HelperMethods.GetCurrentUserAvatarPath(userId);
        }

        public static string Massaged(this string str)
        {
            return str.Trim().ToLower();
        }

        public static void UpdateBalances(this Transaction transaction, int? oldBankAccountId = null)
        {
            if (oldBankAccountId != null)
            {
                var oldBankAccount = 
            }
            // if (transaction != deposit) Budget & Budget Item CurrentAmount += transaction.Amount;
        }

        public static void ManageNotifications(this Transaction transaction)
        {
            var currentBal = transaction.BankAccount.CurrentBalance;
            string householdName = String.Empty;
            string recipientId = String.Empty;
            string subject = String.Empty;
            string notifyBody = String.Empty;
            bool notificationRequired = false;

            if (currentBal < 0)
            {
                notificationRequired = true;
                householdName = transaction.BankAccount.Household.Name;
                recipientId = transaction.BankAccount.OwnerId;
                subject = "Overdraft Notification";
                notifyBody =
                    $"The transaction in the amount of {transaction.Amount} has caused an Over-Draft on Account '{transaction.BankAccount.Name}'";
            }
            if (currentBal >= 0 && currentBal < transaction.BankAccount.LowBalanceLevel)
            {
                notificationRequired = true;
                householdName = transaction.BankAccount.Household.Name;
                recipientId = transaction.BankAccount.OwnerId;
                subject = "Low-Balance Notification";
                notifyBody =
                    $"The transaction in the amount of {transaction.Amount} has caused an Over-Draft on Account '{transaction.BankAccount.Name}'";
            }
            if (notificationRequired)
            {
                CreateNotification(householdName, recipientId, subject, notifyBody);
            }
        }

        private static void CreateNotification(string householdName, string recipientId, string subject, string messageBody)
        {
            var notification = new
            {
                Subject = subject,
                Body = messageBody,
                Created = DateTime.Now,
                HouseholdName = householdName,
                IsRead = false,
                RecipientId = recipientId
            };
        }
    }

    public class Notification
    {

    }
}
