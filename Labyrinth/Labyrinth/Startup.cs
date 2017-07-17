using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Labyrinth.Startup))]
namespace Labyrinth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
