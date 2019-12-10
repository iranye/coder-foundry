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
    [RoutePrefix("Api/BudgetItems")]
    public class BudgetItemsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of BudgetItems for a specific Budget formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [Route("GetBudgetItemsByBudgetId")]
        public async Task<IHttpActionResult> GetBudgetItemsByBudgetId(int bId)
        {
            var data = await _db.GetBudgetItemsByBudgetId(bId);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning a list of BudgetItems for a specific Budget formatted in XML.
        /// </summary>
        /// <returns></returns>
        [Route("GetBudgetItemsByBudgetIdXml")]
        public async Task<List<BudgetItem>> GetBudgetItemsByBudgetIdXml(int bId)
        {
            return await _db.GetBudgetItemsByBudgetId(bId);
        }

        /// <summary>
        /// This is a mechanism for returning an instance of BudgetItem by BudgetItemId formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [Route("GetBudgetItemDetails")]
        public async Task<IHttpActionResult> GetBudgetItemDetails(int biId)
        {
            var data = await _db.GetBudgetItemByBudgetItemId(biId);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning an instance of BudgetItem by BudgetItemId formatted in XML.
        /// </summary>
        /// <returns></returns>
        [Route("GetBudgetItemDetailsXml")]
        public async Task<BudgetItem> GetBudgetItemDetailsXml(int biId)
        {
            return await _db.GetBudgetItemByBudgetItemId(biId);
        }
    }
}
