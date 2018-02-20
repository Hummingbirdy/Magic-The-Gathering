using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MTG.Startup))]
namespace MTG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
