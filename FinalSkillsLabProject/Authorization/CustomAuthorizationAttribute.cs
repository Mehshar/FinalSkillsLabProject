using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace FinalSkillsLabProject.Authorization
{
    public class CustomAuthorizationAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }
        public string[] AuthorizedRoles { get; set; }

        public CustomAuthorizationAttribute(string roles)
        {
            this.Roles = roles;
            this.AuthorizedRoles = this.Roles.Split(',');
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dfController = filterContext.Controller as Controller;

            if(dfController != null && dfController.Session["CurrentRole"] != null)
            {
                var currentRole = dfController.Session["CurrentRole"];
                if(!AuthorizedRoles.Contains(currentRole))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Common", action = "DenyAccess" }));
                }
            }
        }
    }
}