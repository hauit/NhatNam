using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NNworking.Startup))]
namespace NNworking
{
    public partial class Startup
    {
        public static HubConnection HubConnection = new HubConnection("http://www.app.robotech.vn/signalr");
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.MapSignalR();
        }
    }
}
