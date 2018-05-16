using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarePoint.Startup))]
namespace CarePoint
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
