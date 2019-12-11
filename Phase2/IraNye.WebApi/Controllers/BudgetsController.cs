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
    /// Budgets Controller Class
    /// </summary>
    [RoutePrefix("Api/Budgets")]
    public class BudgetsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of Budgets formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<Budget>))]
        [Route("GetBudgetsByHouseholdId")]
        public async Task<IHttpActionResult> GetBudgetsByHouseholdId(int hhId)
        {
            var data = await _db.GetBudgetsByHouseholdId(hhId);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning a list of Budgets formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [Route("GetBudgetsByHouseholdIdXml")]
        public async Task<List<Budget>> GetBudgetsByHouseholdIdXml(int hhId)
        {
            return await _db.GetBudgetsByHouseholdId(hhId);
        }

        /// <summary>
        /// This is a mechanism for returning a Budget instance by BudgetID formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Budget))]
        [Route("GetBudgetDetails")]
        public async Task<IHttpActionResult> GetBudgetDetails(int bId)
        {
            var data = await _db.GetBudgetByBudgetId(bId);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning a Budget instance by BudgetID formatted in XML.
        /// </summary>
        /// <returns></returns>
        [Route("GetBudgetDetailsXml")]
        public async Task<Budget> GetBudgetDetailsXml(int bId)
        {
            return await _db.GetBudgetByBudgetId(bId);
        }

        /// <summary>
        /// This is a mechanism for Adding a new Budget.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Int32))]
        [HttpGet, Route("AddBudget")]
        public IHttpActionResult AddBudget(int hId, string name, string desc, string ownerId, string tgt)
        {
            try
            {
                Guid ownerIdGuid = new Guid(ownerId);
            }
            catch (FormatException e)
            {
                return BadRequest($"Invalid Value for ownerId: '{ownerId}'");
            }

            if (!Decimal.TryParse(tgt, out var tgtAmountDecResult))
            {
                return BadRequest($"Invalid Value for amount: '{tgt}'");
            }
            DateTime created = DateTime.Now;

            var budget = new Budget
            {
                HouseholdId = hId,
                Name = name,
                Description = desc,
                OwnerId = ownerId,
                TargetAmount = tgtAmountDecResult,
                CurrentAmount = 0
            };
            return Ok(_db.AddBudget(budget));
        }
    }
}
