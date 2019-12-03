using FinancialPortal.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace FinancialPortal.Web.ViewModels
{
    public class MainDashboardViewModel
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        //private Household _myHousehold = null;
        //public Household MyHousehold
        //{
        //    get
        //    {
        //        if (_myHousehold == null)
        //        {
        //            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
        //            if (!String.IsNullOrWhiteSpace(currentUserId))
        //            {
        //                var currentUser = _db.Users.Find(currentUserId);
        //                if (currentUser != null)
        //                {
        //                    _myHousehold = _db.Households.Find(currentUser.HouseholdId);
        //                }
        //            }
        //        }

        //        return _myHousehold;
        //    }
        //}

        public Household Household { get; set; }

        public string HouseholdName => Household.Name;

        public List<BankAccount> BankAccounts { get;set; } = new List<BankAccount>();
        public List<Budget> Budgets {get;set; } = new List<Budget>();
        public List<BudgetItem> BudgetItems { get;set; } = new List<BudgetItem>();
        public List<Transaction> Transactions { get;set;} = new List<Transaction>();
        
        public int TotalInvitations
        {
            get { return Household?.Invitations.Count() ?? 0; }
        }

        public int TotalBudgets
        {
            get { return Budgets.Count(); }
        }

        public int TotalBudgetItems
        {
            get { return BudgetItems.Count(); }
        }

        public int TotalBankAccounts
        {
            get { return BankAccounts.Count; }
        }

        public int TotalTransactions
        {
            get { return Transactions.Count; }
        }
    }
}
