using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FinancialPortal.Web.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Web.ViewModels
{
    public class MainDashboardViewModel
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        private Household _myHousehold = null;
        public Household MyHousehold
        {
            get
            {
                if (_myHousehold == null)
                {
                    var currentUserId = HttpContext.Current.User.Identity.GetUserId();
                    if (!String.IsNullOrWhiteSpace(currentUserId))
                    {
                        var currentUser = _db.Users.Find(currentUserId);
                        if (currentUser != null)
                        {
                            _myHousehold = _db.Households.Find(currentUser.HouseholdId);
                        }
                    }
                }

                return _myHousehold;
            }
        }

        public int TotalTransactions
        {
            get { return 50; }
        }

        public int TotalInvitations
        {
            get { return MyHousehold.Invitations.Count(); }
        }

        public int TotalBudgets
        {
            get { return MyHousehold.Budgets.Count(); }
        }
    }
}
