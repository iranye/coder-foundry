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
    }
}
