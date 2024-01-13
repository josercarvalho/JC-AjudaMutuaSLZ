using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JC_AjudaMutuaSLZ.Startup))]
namespace JC_AjudaMutuaSLZ
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
