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
    /// BankAccounts Controller Class
    /// </summary>
    [RoutePrefix("Api/BankAccounts")]
    public class BankAccountsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of BankAccounts for a specific Household formatted in JSON.
        /// </summary>
        /// <param name="hhId">Household Id</param>
        /// <returns>Collection of BankAccounts for specified Household</returns>
        [ResponseType(typeof(List<BankAccount>))]
        [Route("GetBankAccountsByHouseholdId")]
        public async Task<IHttpActionResult> GetBankAccountsByHouseholdId(int hhId)
        {
            var data = await _db.GetAllBankAccountsByHouseholdId(hhId);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning a list of BankAccounts for a specific Household formatted in XML.
        /// </summary>
        /// <param name="hhId">Household Id</param>
        /// <returns>Collection of BankAccounts</returns>
        [Route("GetBankAccountsByHouseholdIdXml")]
        public async Task<List<BankAccount>> GetBankAccountsByHouseholdIdXml(int hhId)
        {
            return await _db.GetAllBankAccountsByHouseholdId(hhId);
        }

        /// <summary>
        /// This is a mechanism for returning an instance of BankAccount by BankAccountId formatted in JSON.
        /// </summary>
        /// <param name="id">Bank Account Id</param>
        /// <returns>BankAccount</returns>
        [ResponseType(typeof(BankAccount))]
        [Route("GetBankAccountDetails")]
        public async Task<IHttpActionResult> GetBankAccountDetails(int id)
        {
            var data = await _db.GetBankAccountByBankAccountId(id);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning an instance of BankAccount by BankAccountId formatted in XML.
        /// </summary>
        /// <param name="id">Bank Account Id</param>
        /// <returns>BankAccount</returns>
        [Route("GetBankAccountDetailsXml")]
        public async Task<BankAccount> GetBankAccountDetailsXml(int id)
        {
            return await _db.GetBankAccountByBankAccountId(id);
        }

        /// <summary>
        /// This is a mechanism for Adding a new Bank Account.
        /// </summary>
        /// <returns></returns>
        /// <param name="hId">Household Id</param>
        /// <param name="name">Bank Account Name</param>
        /// <param name="type">Bank Account Type</param>
        /// <param name="startBal">Starting Account Balance</param>
        /// <param name="lowBal">Low Balance Threshold Amount</param>
        /// <param name="ownerId">Bank Account Owner Id (Guid)</param>
        [ResponseType(typeof(IHttpActionResult))]
        [HttpGet, HttpPost, Route("AddBankAccount")]
        public IHttpActionResult AddBankAccount(int hId, string name, AccountType type, float startBal, float lowBal, string ownerId)
        {
            try
            {
                Guid ownerIdGuid = new Guid(ownerId);
            }
            catch (FormatException e)
            {
                return BadRequest($"Invalid Value for ownerId: '{ownerId}'");
            }

            if (!Enum.IsDefined(typeof(AccountType), type))
            {
                return BadRequest($"Invalid Value for AccountType: '{type}'");
            }

            var startBalDecResult = Convert.ToDecimal(Math.Round(startBal, 2));
            var lowBalDecResult = Convert.ToDecimal(Math.Round(lowBal, 2));
            DateTime created = DateTime.Now;

            var bankAccount = new BankAccount
            {
                HouseholdId = hId,
                Name = name,
                AccountType = type,
                StartingBalance = startBalDecResult,
                CurrentBalance = startBalDecResult,
                LowBalanceLevel = lowBalDecResult,
                OwnerId = ownerId
            };
            return Ok(_db.AddBankAccount(bankAccount));
        }
    }
}
