using System;
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

        public async Task<List<BudgetItem>> GetBudgetItemsByBudgetId(int bId)
        {
            SqlParameter param1 = new SqlParameter("@budgetId", bId);
            return await Database.SqlQuery<BudgetItem>("GetAllBudgetItemsByBudgetId @budgetId", param1).ToListAsync();
        }

        public async Task<BudgetItem> GetBudgetItemByBudgetItemId(int budgetItemId)
        {
            SqlParameter param1 = new SqlParameter("@id", budgetItemId);
            return await Database.SqlQuery<BudgetItem>("GetBudgetItemByBudgetItemId @id", param1).FirstOrDefaultAsync();
        }

        public async Task<List<BankAccount>> GetAllBankAccountsByHouseholdId(int hhId)
        {
            SqlParameter param1 = new SqlParameter("@householdId", hhId);
            return await Database.SqlQuery<BankAccount>("GetAllBankAccountsByHouseholdId @householdId", param1).ToListAsync();
        }

        public async Task<BankAccount> GetBankAccountByBankAccountId(int bankAccountId)
        {
            SqlParameter param1 = new SqlParameter("@id", bankAccountId);
            return await Database.SqlQuery<BankAccount>("GetBankAccountByBankAccountId @id", param1).FirstOrDefaultAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByBankAccountId(int bankAccountId)
        {
            SqlParameter param1 = new SqlParameter("@bankAccountId", bankAccountId);
            return await Database.SqlQuery<Transaction>("GetAllTransactionsByBankAccountId @bankAccountId", param1).ToListAsync();
        }

        public async Task<Transaction> GetTransactionByTransactionId(int transactionId)
        {
            SqlParameter param1 = new SqlParameter("@id", transactionId);
            return await Database.SqlQuery<Transaction>("GetTransactionByTransactionId @id", param1).FirstOrDefaultAsync();
        }

        //public async Task<Transaction> AddTransaction(int bankAccountId, int budgetItemId, int transactionTypeId, string createdById,
        //    decimal amount, DateTime created, string memo)
        //{
        //    SqlParameter param001 = new SqlParameter("@bankAccountId", bankAccountId);
        //    SqlParameter param002 = new SqlParameter("@budgetItemId", budgetItemId);
        //    SqlParameter param003 = new SqlParameter("@transactionTypeId", transactionTypeId);
        //    SqlParameter param004 = new SqlParameter("@createdById", createdById);
        //    SqlParameter param005 = new SqlParameter("@amount", amount);
        //    SqlParameter param006 = new SqlParameter("@created", created);
        //    SqlParameter param007 = new SqlParameter("@memo", memo);

        //    return await Database.SqlQuery<Transaction>("GetTransactionByTransactionId @bankAccountId,
        //    @budgetItemId,
        //    @transactionTypeId,
        //    @createdById,
        //    @amount,
        //    @created,
        //    @memo", param1).FirstOrDefaultAsync();
        //}

        public int AddTransaction(int bankAccountId, int budgetItemId, int transactionTypeId, string createdById,
            decimal amount, DateTime created, string memo)
        {
            SqlParameter param1 = new SqlParameter("@bankAccountId", bankAccountId);
            SqlParameter param2 = new SqlParameter("@budgetItemId", budgetItemId);
            SqlParameter param3 = new SqlParameter("@transactionTypeId", transactionTypeId);
            SqlParameter param4 = new SqlParameter("@createdById", createdById);
            SqlParameter param5 = new SqlParameter("@amount", amount);
            SqlParameter param6 = new SqlParameter("@created", created);
            SqlParameter param7 = new SqlParameter("@memo", memo);

            //var ret = Database.SqlQuery<Transaction>("GetTransactionByTransactionId @bankAccountId, @budgetItemId, @transactionTypeId, @createdById," +
            //"@amount, @created, @memo", param1, param2, param3, param4, param5, param6, param7).FirstOrDefaultAsync();
            
            // TODO: Parameterize any & all user input
            var ret = Database.ExecuteSqlCommand("GetTransactionByTransactionId @bankAccountId, @budgetItemId, @transactionTypeId, @createdById," +
                                                 "@amount, @created, @memo", param1, param2, param3, param4, param5, param6, param7);
            return ret;
        }

        public int AddHousehold(string name, string greeting)
        {
            SqlParameter param1 = new SqlParameter("@name", name);
            SqlParameter param2 = new SqlParameter("@greeting", greeting);

            return Database.ExecuteSqlCommand("AddHousehold @name, @greeting", param1, param2);
        }

        /*

        AddTransaction
        AddBankAccount
        AddBudget
        
        */

    }
}

