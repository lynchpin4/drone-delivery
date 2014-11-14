using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DroneDelivery.Startup))]
namespace DroneDelivery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
