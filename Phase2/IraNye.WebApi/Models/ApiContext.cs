using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using Newtonsoft.Json;

namespace IraNye.WebApi.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext()
            : base("DefaultConnection")
        {
        }

        public static ApiContext Create()
        {
            return new ApiContext();
        }

        public async Task<Household> GetHouseholdById(int hhId)
        {
            SqlParameter param1 = new SqlParameter("@id", hhId);
            return await Database.SqlQuery<Household>("GetHouseholdById @id", param1).FirstOrDefaultAsync();
        }

        public async Task<List<Household>> GetAllHouseholds()
        {
            return await Database.SqlQuery<Household>("GetAllHouseholds").ToListAsync();
        }
    }
}
