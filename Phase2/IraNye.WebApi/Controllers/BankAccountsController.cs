using IraNye.WebApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace IraNye.WebApi.Controllers
{
    [RoutePrefix("Api/BankAccounts")]
    public class BankAccountsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of BankAccounts for a specific Household formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [Route("GetBankAccountsByHouseholdId")]
        public async Task<IHttpActionResult> GetBankAccountsByHouseholdId(int hhId)
        {
            var data = await _db.GetAllBankAccountsByHouseholdId(hhId);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning a list of BankAccounts for a specific Household formatted in XML.
        /// </summary>
        /// <returns></returns>
        [Route("GetBankAccountsByHouseholdIdXml")]
        public async Task<List<BankAccount>> GetBankAccountsByHouseholdIdXml(int hhId)
        {
            return await _db.GetAllBankAccountsByHouseholdId(hhId);
        }

        /// <summary>
        /// This is a mechanism for returning an instance of BankAccount by BankAccountId formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [Route("GetBankAccountDetails")]
        public async Task<IHttpActionResult> GetBankAccountDetails(int id)
        {
            var data = await _db.GetBankAccountByBankAccountId(id);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning an instance of BankAccount by BankAccountId formatted in XML.
        /// </summary>
        /// <returns></returns>
        [Route("GetBankAccountDetailsXml")]
        public async Task<BankAccount> GetBankAccountDetailsXml(int id)
        {
            return await _db.GetBankAccountByBankAccountId(id);
        }
    }
}
