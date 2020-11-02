using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ShopWay
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                GlobalConfiguration.Configure(WebApiConfig.Register);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
