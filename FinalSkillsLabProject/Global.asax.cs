﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
    }
}
