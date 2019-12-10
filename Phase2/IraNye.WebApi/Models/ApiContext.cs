using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IraNye.WebApi.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext()
            : base("DefaultConnection")
        {
        }

        public static ApiContext Create()
        {
            return new ApiContext();
        }

        public async Task<List<Household>> GetAllHouseholds()
        {
            return await Database.SqlQuery<Household>("GetAllHouseholds").ToListAsync();
        }

        public async Task<Household> GetHouseholdById(int hhId)
        {
            SqlParameter param1 = new SqlParameter("@id", hhId);
            return await Database.SqlQuery<Household>("GetHouseholdById @id", param1).FirstOrDefaultAsync();
        }

        public async Task<List<Budget>> GetBudgetsByHouseholdId(int hhId)
        {
            SqlParameter param1 = new SqlParameter("@hhId", hhId);
            return await Database.SqlQuery<Budget>("GetBudgetsByHouseholdId @hhId", param1).ToListAsync();
        }

        public async Task<Budget> GetBudgetByBudgetId(int budgetId)
        {
            SqlParameter param1 = new SqlParameter("@id", budgetId);
            return await Database.SqlQuery<Budget>("GetBudgetByBudgetId @id", param1).FirstOrDefaultAsync();
        }

        /*

        GetAllTransactionsByBankAccountId
        AddTransaction
        AddBankAccount
        AddBudget

        GetBudgetsByHouseholdId
        GetAllBudgetItemsByHouseholdId
        GetAllBudgetItemsByBudgetId
        GetAllBankAccountsByHouseholdId

         */

        public async Task<BankAccount> GetBankAccountByBankAccountId(int bankAccountId)
        {
            SqlParameter param1 = new SqlParameter("@id", bankAccountId);
            return await Database.SqlQuery<BankAccount>("GetBankAccountByBankAccountId @id", param1).FirstOrDefaultAsync();
        }

        public async Task<BudgetItem> GetBudgetItemByBudgetItemId(int budgetItemId)
        {
            SqlParameter param1 = new SqlParameter("@id", budgetItemId);
            return await Database.SqlQuery<BudgetItem>("GetBudgetItemByBudgetItemId @id", param1).FirstOrDefaultAsync();
        }

        public async Task<Transaction> GetTransactionByTransactionId(int transactionId)
        {
            SqlParameter param1 = new SqlParameter("@id", transactionId);
            return await Database.SqlQuery<Transaction>("GetTransactionByTransactionId @id", param1).FirstOrDefaultAsync();
        }

    }
}

