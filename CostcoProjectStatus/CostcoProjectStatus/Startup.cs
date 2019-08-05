using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CostcoProjectStatus.Startup))]
namespace CostcoProjectStatus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
