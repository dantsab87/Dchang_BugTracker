using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dchang_BugTracker.Startup))]
namespace Dchang_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
