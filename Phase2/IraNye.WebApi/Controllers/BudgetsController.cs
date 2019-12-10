using IraNye.WebApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace IraNye.WebApi.Controllers
{
    [RoutePrefix("Api/Budgets")]
    public class BudgetsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of Budgets formatted in JSON.
        /// </summary>
        /// <returns></returns>
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

    }
}
