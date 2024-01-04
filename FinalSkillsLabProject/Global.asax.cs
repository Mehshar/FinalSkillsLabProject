using FinalSkillsLabProject.Common.ErrorLogging;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace FinalSkillsLabProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            // Using the Authorize filter at a global level
            // To prevent the 401 Unauthorized error, we make use of AllowAnonymous
            GlobalFilters.Filters.Add(new AuthorizeAttribute());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();

        //    ILogger logger = UnityConfig.UnityContainer.Resolve<ILogger>();
        //    logger.LogError(exception);

        //    // Clear the error to prevent ASP.NET from handling it
        //    Server.ClearError();

        //    CustomErrorsSection customErrors = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");

        //    int statusCode = (exception is HttpException) ? ((HttpException)exception).GetHttpCode() : 500;
        //    string redirectPath = customErrors.Errors.OfType<CustomError>()
        //        .FirstOrDefault(error => error.StatusCode == statusCode)
        //        ?.Redirect;
        //    Response.Redirect(redirectPath ?? customErrors.DefaultRedirect);
        //}
    }
}
