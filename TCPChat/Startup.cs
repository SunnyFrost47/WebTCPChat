using Microsoft.Owin;
using Owin;
using System.Net.WebSockets;

[assembly: OwinStartupAttribute(typeof(TCPChat.Startup))]
namespace TCPChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            //app.UseWebSockets();
        }
    }
}
