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

        public static bool UpdateBalances(this Transaction transaction, bool isDeleted = false, decimal oldAmount = 0m)
        {
            bool updatesMade = false;
            if (transaction.Amount != 0)
            {
                if (isDeleted)
                {
                    // If Deposit, Update BankAccount.CurrentBalance with Minus, else Add
                    if (transaction.TransactionType.Type.ToUpper() == "DEPOSIT")
                    {
                        transaction.BankAccount.CurrentBalance -= transaction.Amount;
                        updatesMade = true;
                    }
                    else
                    {
                        transaction.BankAccount.CurrentBalance += transaction.Amount;
                        if (transaction.BudgetItem != null)
                        {
                            transaction.BudgetItem.CurrentAmount -= transaction.Amount;
                        }
                        updatesMade = true;
                    }
                }
                else
                {
                    // If Deposit, Update BankAccount.CurrentBalance with Add, else Minus
                    if (transaction.TransactionType.Type.ToUpper() == "DEPOSIT")
                    {
                        if (oldAmount > 0)
                        {
                            transaction.BankAccount.CurrentBalance -= oldAmount;
                        }
                        transaction.BankAccount.CurrentBalance += transaction.Amount;
                        updatesMade = true;
                    }
                    else
                    {
                        if (oldAmount > 0)
                        {
                            transaction.BankAccount.CurrentBalance += oldAmount;
                        }
                        transaction.BankAccount.CurrentBalance -= transaction.Amount;
                        if (transaction.BudgetItem != null)
                        {
                            if (oldAmount > 0)
                            {
                                transaction.BudgetItem.CurrentAmount -= transaction.Amount;
                            }
                            transaction.BudgetItem.CurrentAmount += transaction.Amount;
                        }
                        updatesMade = true;
                    }
                }
            }

            return updatesMade;
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
