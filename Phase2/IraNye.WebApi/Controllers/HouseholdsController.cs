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
    /// Households Controller Class
    /// </summary>
    [RoutePrefix("Api/Households")]
    public class HouseholdsController : ApiController
    {
        private readonly ApiContext _db = new ApiContext();

        /// <summary>
        /// This is a mechanism for returning a list of Households formatted in JSON.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(List<Household>))]
        [Route("GetAllHouseholds")]
        public async Task<IHttpActionResult> GetAllHouseholds()
        {
            var data = await _db.GetAllHouseholds();
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
        [ResponseType(typeof(Household))]
        [Route("GetHouseholdDetails")]
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
        [Route("GetHouseholdDetailsXml")]
        public async Task<Household> GetHouseholdXml(int id)
        {
            return await _db.GetHouseholdById(id);
        }

        /// <summary>
        /// This is a mechanism to add a new Household.
        /// </summary>
        [ResponseType(typeof(Int32))]
        [HttpGet, Route("AddHousehold")]
        public IHttpActionResult AddHousehold(string name, string greeting)
        // public async Task<int> AddHousehold(string name, string greeting)
        {
            return Ok(_db.AddHousehold(name, greeting));
        }


    }
}
