using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAMvc.Startup))]
namespace SAMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
