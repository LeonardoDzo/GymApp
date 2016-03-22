using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GymApp.Startup))]
namespace GymApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
