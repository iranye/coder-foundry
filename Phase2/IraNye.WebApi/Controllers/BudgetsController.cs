using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using IraNye.WebApi.Models;
using Newtonsoft.Json;

namespace IraNye.WebApi.Controllers
{
    [RoutePrefix("Api/Budgets")]
    public class BudgetsController : ApiController
    {
        private ApiContext _db = new ApiContext();

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
        /// This is a mechanism for returning a list of Households formatted in XML.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllHouseholdsXml")]
        public async Task<List<Household>> GetAllHouseholdsXml()
        {
            return await _db.GetAllHouseholds();
        }

        /// <summary>
        /// This is a mechanism for returning Details for a specific Household formatted in JSON.
        /// </summary>
        /// <param name="id">Primary Key of Household</param>
        [Route("GetHousehold")]
        public async Task<IHttpActionResult> GetHousehold(int id)
        {
            var data = await _db.GetHouseholdById(id);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning Details for a specific Household formatted in XML.
        /// </summary>
        /// <param name="id">Primary Key of Household</param>
        /// <returns></returns>
        [Route("GetHouseholdXml")]
        public async Task<Household> GetHouseholdXml(int id)
        {
            return await _db.GetHouseholdById(id);
        }

    }
}
