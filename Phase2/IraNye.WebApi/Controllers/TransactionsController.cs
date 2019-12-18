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
        /// <param name="baId">Bank Account Id</param>
        /// <returns>Collection of Transactions</returns>
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
        /// <param name="baId">Bank Account Id</param>
        /// <returns>Collection of Transactions</returns>
        [Route("GetTransactionsByBankAccountIdXml")]
        public async Task<List<Transaction>> GetTransactionsByBankAccountIdXml(int baId)
        {
            return await _db.GetTransactionsByBankAccountId(baId);
        }

        /// <summary>
        /// This is a mechanism for returning an instance of Transaction by TransactionId formatted in JSON.
        /// </summary>
        /// <param name="id">Transaction Id</param>
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
        /// <param name="id">Transaction Id</param>
        /// <returns>Transaction</returns>
        [Route("GetTransactionDetailsXml")]
        public async Task<Transaction> GetTransactionDetailsXml(int id)
        {
            return await _db.GetTransactionByTransactionId(id);
        }

        /// <summary>
        /// This is a mechanism for Adding a new Transaction.
        /// </summary>
        /// <param name="bId">Bank Account Id</param>
        /// <param name="biId">Budget Item Id</param>
        /// <param name="ttId">Transaction Type Id</param>
        /// <param name="createdById">Transaction Created-By Owner Id (Guid)</param>
        /// <param name="amount">Transaction Amount</param>
        /// <param name="transactionDateTime">Transaction Date & Time</param>
        /// <param name="memo">Transaction Memo</param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IHttpActionResult))]
        [HttpGet, HttpPost, Route("AddTransaction")]
        public IHttpActionResult AddTransaction(int bId, int biId, int ttId, string createdById, float amount, string transactionDateTime, string memo)
        {
            try
            {
                Guid createdByGuid = new Guid(createdById);
            }
            catch (FormatException e)
            {
                return BadRequest($"Invalid Value for createdById: '{createdById}'");
            }
            var amountDecResult = Convert.ToDecimal(Math.Round(amount, 2));
            DateTime transactionDateTimeParsed = DateTime.Parse(transactionDateTime);

            var transaction = new Transaction
            {
                BankAccountId = bId,
                BudgetItemId = biId,
                TransactionTypeId = ttId,
                CreatedById = createdById,
                Amount = amountDecResult,
                TransactionDateTime = transactionDateTimeParsed,
                Memo = memo
            };
            return Ok(_db.AddTransaction(transaction));
        }

        /// <summary>
        /// This is a mechanism for Updating an existing Transaction.
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <param name="bId">Bank Account Id</param>
        /// <param name="biId">Budget Item Id</param>
        /// <param name="ttId">Transaction Type Id</param>
        /// <param name="createdById">Transaction Created-By Owner Id (Guid)</param>
        /// <param name="amount">Transaction Amount</param>
        /// <param name="transactionDateTime">Transaction Date & Time</param>
        /// <param name="memo">Transaction Memo</param>
        /// <returns>IHttpActionResult</returns>
        [ResponseType(typeof(IHttpActionResult))]
        [HttpPut, Route("UpdateTransaction")]
        public IHttpActionResult UpdateTransaction(int id, int bId, int biId, int ttId, string createdById, float amount, string transactionDateTime, string memo)
        {
            try
            {
                Guid createdByGuid = new Guid(createdById);
            }
            catch (FormatException e)
            {
                return BadRequest($"Invalid Value for createdById: '{createdById}'");
            }
            var amountDecResult = Convert.ToDecimal(Math.Round(amount, 2));
            DateTime transactionDateTimeParsed = DateTime.Parse(transactionDateTime);

            var transaction = new Transaction
            {
                Id = id,
                BankAccountId = bId,
                BudgetItemId = biId,
                TransactionTypeId = ttId,
                CreatedById = createdById,
                Amount = amountDecResult,
                TransactionDateTime = transactionDateTimeParsed,
                Memo = memo
            };
            return Ok(_db.UpdateTransaction(transaction));
        }

        /// <summary>
        /// This is a mechanism for Deleting a Transaction by TransactionId.
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <returns>Transaction</returns>
        [ResponseType(typeof(IHttpActionResult))]
        [HttpDelete, Route("DeleteTransaction")]
        public IHttpActionResult DeleteTransaction(int id)
        {
            return Ok(_db.DeleteTransactionByTransactionId(id));
        }
    }
}
