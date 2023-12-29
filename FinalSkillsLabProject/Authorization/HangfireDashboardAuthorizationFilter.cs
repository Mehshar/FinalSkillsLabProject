using FinalSkillsLabProject.Common.Enums;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalSkillsLabProject.Authorization
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            string userRole = HttpContext.Current.Session["CurrentRole"]?.ToString() ?? "";
            bool isAuthorized = userRole.Equals(RoleEnum.Admin.ToString(), StringComparison.OrdinalIgnoreCase);

            if (!isAuthorized)
            {
                HttpContext.Current.Response.Redirect("/Common/DenyAccess");
                return false;
            }
            return true;
        }
    }
}