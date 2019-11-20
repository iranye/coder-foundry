using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinancialPortal.Web.Startup))]
namespace FinancialPortal.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
