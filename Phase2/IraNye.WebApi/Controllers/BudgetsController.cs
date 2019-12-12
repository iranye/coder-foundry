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
        /// <param name="hhId">Household Id</param>
        /// <returns>Collection of Budgets for specified Household</returns>
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
        /// <param name="hhId">Household Id</param>
        /// <returns>Collection of Budgets for specified Household</returns>
        [Route("GetBudgetsByHouseholdIdXml")]
        public async Task<List<Budget>> GetBudgetsByHouseholdIdXml(int hhId)
        {
            return await _db.GetBudgetsByHouseholdId(hhId);
        }

        /// <summary>
        /// This is a mechanism for returning a Budget instance by BudgetID formatted in JSON.
        /// </summary>
        /// <param name="id">Budget Id</param>
        /// <returns>Budget</returns>
        [ResponseType(typeof(Budget))]
        [Route("GetBudgetDetails")]
        public async Task<IHttpActionResult> GetBudgetDetails(int id)
        {
            var data = await _db.GetBudgetByBudgetId(id);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning a Budget instance by BudgetID formatted in XML.
        /// </summary>
        /// <param name="id">Budget Id</param>
        /// <returns>Budget</returns>
        [Route("GetBudgetDetailsXml")]
        public async Task<Budget> GetBudgetDetailsXml(int id)
        {
            return await _db.GetBudgetByBudgetId(id);
        }

        /// <summary>
        /// This is a mechanism for Adding a new Budget.
        /// </summary>
        /// <param name="hId">Household Id</param>
        /// <param name="name">Budget Name</param>
        /// <param name="desc">Budget Description</param>
        /// <param name="ownerId">Budget Owner ID (Guid)</param>
        /// <param name="tgt">Budget Target Amount</param>
        [ResponseType(typeof(IHttpActionResult))]
        [HttpGet, HttpPost, Route("AddBudget")]
        public IHttpActionResult AddBudget(int hId, string name, string desc, string ownerId, float tgt)
        {
            try
            {
                Guid ownerIdGuid = new Guid(ownerId);
            }
            catch (FormatException e)
            {
                return BadRequest($"Invalid Value for ownerId: '{ownerId}'");
            }

            var targetDecResult = Convert.ToDecimal(Math.Round(tgt, 2, MidpointRounding.AwayFromZero));
            DateTime created = DateTime.Now;

            var budget = new Budget
            {
                HouseholdId = hId,
                Name = name,
                Description = desc,
                OwnerId = ownerId,
                TargetAmount = targetDecResult,
                CurrentAmount = 0
            };
            return Ok(_db.AddBudget(budget));
        }
    }
}
