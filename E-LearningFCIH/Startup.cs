using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_LearningFCIH.Startup))]
namespace E_LearningFCIH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
