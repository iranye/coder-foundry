using IraNye.WebApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace IraNye.WebApi.Controllers
{
    /// <summary>
    /// BudgetItems Controller Class
    /// </summary>
    [RoutePrefix("Api/BudgetItems")]
    public class BudgetItemsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of BudgetItems for a specific Budget formatted in JSON.
        /// </summary>
        /// <param name="bId">Budget Id</param>
        /// <returns>Collection of BudgetItems</returns>
        [ResponseType(typeof(List<BudgetItem>))]
        [Route("GetBudgetItemsByBudgetId")]
        public async Task<IHttpActionResult> GetBudgetItemsByBudgetId(int bId)
        {
            var data = await _db.GetBudgetItemsByBudgetId(bId);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning a list of BudgetItems for a specific Budget formatted in XML.
        /// </summary>
        /// <param name="bId">Budget Id</param>
        /// <returns>Collection of BudgetItems</returns>
        [Route("GetBudgetItemsByBudgetIdXml")]
        public async Task<List<BudgetItem>> GetBudgetItemsByBudgetIdXml(int bId)
        {
            return await _db.GetBudgetItemsByBudgetId(bId);
        }

        /// <summary>
        /// This is a mechanism for returning an instance of BudgetItem by BudgetItemId formatted in JSON.
        /// </summary>
        /// <param name="id">BudgetItem Id</param>
        /// <returns>BudgetItem</returns>
        [ResponseType(typeof(BudgetItem))]
        [Route("GetBudgetItemDetails")]
        public async Task<IHttpActionResult> GetBudgetItemDetails(int id)
        {
            var data = await _db.GetBudgetItemByBudgetItemId(id);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        /// <summary>
        /// This is a mechanism for returning an instance of BudgetItem by BudgetItemId formatted in XML.
        /// </summary>
        /// <param name="id">BudgetItem Id</param>
        /// <returns>BudgetItem</returns>
        [Route("GetBudgetItemDetailsXml")]
        public async Task<BudgetItem> GetBudgetItemDetailsXml(int id)
        {
            return await _db.GetBudgetItemByBudgetItemId(id);
        }
    }
}
