using System;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Banking.Membership;

namespace Banking
{
    using System.Linq;
    using System.Web;

    using Banking.Application.Core.Logging;
    using Banking.Application.DAL;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static SimpleMembershipInitializer _initializer;

        private static object _initializerLock = new object();

        private static bool _isInitialized;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);

            var razorEngine = ViewEngines.Engines.OfType<RazorViewEngine>().First();

            razorEngine.ViewLocationFormats = new string[] { "~/Application/Web/Views/{1}/{0}.cshtml" };

            razorEngine.PartialViewLocationFormats = new string[] { "~/Application/Web/Views/{1}/{0}.cshtml" };
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (Response.StatusCode != 404)
            {
                // Instantiating here for now
                ILogger logger = new Logger(new LoggerRepository());

                logger.LogException(ex);
                var errorPageUrl = string.Format("~/Error/{0}/?message={1}", "General", "Something went wrong");
                Server.ClearError();

                Response.Redirect(errorPageUrl);
            }
        }
    }
}