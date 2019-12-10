using System;
using IraNye.WebApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace IraNye.WebApi.Controllers
{
    [RoutePrefix("Api/Transactions")]
    public class TransactionsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of Transactions for a specific BankAccount formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [Route("GetTransactionsByBankAccountId")]
        public async Task<IHttpActionResult> GetTransactionsByBankAccountId(int baId)
        {
            var data = await _db.GetTransactionsByBankAccountId(baId);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning a list of Transactions for a specific BankAccount formatted in XML.
        /// </summary>
        /// <returns></returns>
        [Route("GetTransactionsByBankAccountIdXml")]
        public async Task<List<Transaction>> GetTransactionsByBankAccountIdXml(int baId)
        {
            return await _db.GetTransactionsByBankAccountId(baId);
        }

        /// <summary>
        /// This is a mechanism for returning an instance of Transaction by TransactionId formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [Route("GetTransactionDetails")]
        public async Task<IHttpActionResult> GetTransactionDetails(int id)
        {
            var data = await _db.GetTransactionByTransactionId(id);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning an instance of Transaction by TransactionId formatted in XML.
        /// </summary>
        /// <returns></returns>
        [Route("GetTransactionDetailsXml")]
        public async Task<Transaction> GetTransactionDetailsXml(int id)
        {
            return await _db.GetTransactionByTransactionId(id);
        }

        /// <summary>
        /// This is a mechanism for Adding a new instance of Transaction.
        /// </summary>
        /// <returns></returns>
        [Route("AddTransaction")]
        public IHttpActionResult AddTransaction()
        {
            //10,	2, 9,	'cbfd605b-e7de-4ab1-9404-71db3477bafa', 385.00,	'Weekly Rate'
            int bankAccountId = 10;
            int budgetItemId = 2;
            int transactionTypeId = 9;
            //Guid createdById = new Guid('cbfd605b-e7de-4ab1-9404-71db3477bafa');
            string createdById = "cbfd605b-e7de-4ab1-9404-71db3477bafa";
            decimal amount = 385.00m;
            DateTime created = DateTime.Now;
            string memo = "Weekly Rate";
            var transaction = new Transaction
            {
                BankAccountId = bankAccountId,
                BudgetItemId = budgetItemId,
                TransactionTypeId = transactionTypeId,
                CreatedById = createdById,
                Amount = amount,
                Created = DateTime.Now,
                Memo = memo
            };
            return Ok(_db.AddTransaction(bankAccountId, budgetItemId, transactionTypeId, createdById, amount, created, memo));
        }
    }
}
