using FinalSkillsLabProject.Common.Enums;
using FinalSkillsLabProject.Common.Models;
using Hangfire.Dashboard;
using System;
using System.Web;

namespace FinalSkillsLabProject.Authorization
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentUser"] != null)
            {
                string userRole = ((UserViewModel)HttpContext.Current.Session["CurrentUser"]).Role.RoleName.ToString();
                bool isAuthorized = userRole.Equals(RoleEnum.Admin.ToString(), StringComparison.OrdinalIgnoreCase);

                if (!isAuthorized)
                {
                    HttpContext.Current.Response.Redirect("/Common/DenyAccess");
                    return false;
                }

                return true;
            }

            HttpContext.Current.Response.Redirect("/Account/Login");
            return false;
        }

    }
}