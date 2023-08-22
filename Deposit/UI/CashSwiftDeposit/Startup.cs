
using Owin;
using System.Web.Http;

namespace CashSwiftDeposit
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration configuration = new HttpConfiguration();
            configuration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new
            {
                id = RouteParameter.Optional
            });
            appBuilder.UseWebApi(configuration);
        }
    }
}
