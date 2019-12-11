using System;
using IraNye.WebApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace IraNye.WebApi.Controllers
{
    /// <summary>
    /// Transactions Controller Class
    /// </summary>
    [RoutePrefix("Api/Transactions")]
    public class TransactionsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of Transactions for a specific BankAccount formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<Transaction>))]
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
        /// <returns>Transaction</returns>
        [ResponseType(typeof(Transaction))]
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
        /// This is a mechanism for Adding a new Transaction.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Int32))]
        [HttpGet, Route("AddTransaction")]
        public IHttpActionResult AddTransaction(int bId, int biId, int ttId, string createdById, string amount, string memo)
        {
            int bankAccountId = bId;
            int budgetItemId = biId;
            int transactionTypeId = ttId;
            try
            {
                Guid createdByGuid = new Guid(createdById);
            }
            catch (FormatException e)
            {
                return BadRequest($"Invalid Value for createdById: '{createdById}'");
            }
            if (!Decimal.TryParse(amount, out var amountDecResult))
            {
                return BadRequest($"Invalid Value for amount: '{amount}'");
            }
            DateTime created = DateTime.Now;

            var transaction = new Transaction
            {
                BankAccountId = bankAccountId,
                BudgetItemId = budgetItemId,
                TransactionTypeId = transactionTypeId,
                CreatedById = createdById,
                Amount = amountDecResult,
                Created = DateTime.Now,
                Memo = memo
            };
            return Ok(_db.AddTransaction(transaction));
        }
    }
}
