using FinancialPortal.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace FinancialPortal.Web.ViewModels
{
    public class MainDashboardViewModel
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        
        public Household Household { get; set; }

        public string HouseholdName => Household.Name;

        public List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
        public List<Budget> Budgets { get; set; } = new List<Budget>();
        public List<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public MainDashboardViewModel()
        {
            var currentUserHouseholdId = Helpers.HelperMethods.GetCurrentUserHouseholdId();
            if (currentUserHouseholdId != null)
            {
                Household = _db.Households.AsNoTracking().FirstOrDefault(h => h.Id == currentUserHouseholdId);

                BankAccounts.AddRange(_db.BankAccounts.AsNoTracking().Where(b => b.HouseholdId == currentUserHouseholdId).ToList());
                Budgets.AddRange(_db.Budgets.AsNoTracking().Where(b => b.HouseholdId == currentUserHouseholdId).ToList());

                BudgetItems.AddRange(Budgets.SelectMany(b => b.BudgetItems));

                var transactions = _db.Transactions.AsNoTracking();
                foreach (var transaction in transactions)
                {
                    if (BankAccounts.Any(ba => ba.Id == transaction.BankAccountId))
                    {
                        Transactions.Add(transaction);
                    }
                }
            }
        }

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
