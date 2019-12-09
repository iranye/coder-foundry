using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IraNye.WebApi.Models;
using Newtonsoft.Json;

namespace IraNye.WebApi.Controllers
{
    [RoutePrefix("Api/Households")]
    public class HouseholdsController : ApiController
    {
        private ApiContext _db = new ApiContext();

        [Route("GetAllHouseholds")]
        public async Task<List<Household>> GetAllHouseholds()
        {
            return await _db.GetAllHouseholds();
        }

        [Route("GetAllHouseholdsAsJson")]
        public async Task<IHttpActionResult> GetAllHouseholdsAsJson()
        {
            var data = await _db.GetAllHouseholds();
            return Json(data, new JsonSerializerSettings {Formatting = Formatting.Indented});
        }

        [Route("GetHousehold")]
        public async Task<Household> GetHousehold(int id)
        {
            return await _db.GetHouseholdById(id);
        }

        [Route("GetHouseholdAsJson")]
        public async Task<IHttpActionResult> GetHouseholdAsJson(int id)
        {
            var data = await _db.GetHouseholdById(id);
            return Json(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

    }
}
