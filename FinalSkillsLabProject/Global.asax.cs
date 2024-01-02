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

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            ILogger logger = UnityConfig.UnityContainer.Resolve<ILogger>();
            //CustomException customException = new CustomException("Global Application Error", exception, logger);
            //customException.Log();
            logger.LogError(exception);

            // Clear the error to prevent ASP.NET from handling it
            Server.ClearError();

            // Get the customErrors section from the web.config file
            CustomErrorsSection customErrors = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");

            // Get the status code of the error
            int statusCode = (exception is HttpException) ? ((HttpException)exception).GetHttpCode() : 500;
            // Find the corresponding error redirect from the customErrors section
            string redirectPath = customErrors.Errors.OfType<CustomError>()
                .FirstOrDefault(error => error.StatusCode == statusCode)
                ?.Redirect;
            // Redirect to the specified location or use the defaultRedirect
            Response.Redirect(redirectPath ?? customErrors.DefaultRedirect);
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();

        //    ILogger logger = UnityConfig.UnityContainer.Resolve<ILogger>();
        //    CustomException customException = new CustomException("Global Application Error", exception, logger);
        //    customException.Log();

        //    // Clear the error to prevent ASP.NET from handling it
        //    Server.ClearError();

        //    // Get the customErrors section from the web.config file
        //    CustomErrorsSection customErrors = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");
        //    Response.RedirectToRoute(customErrors.DefaultRedirect);
        //}
    }
}
