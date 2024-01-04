using System.Web.Mvc;

namespace FinalSkillsLabProject.Controllers
{
    [AllowAnonymous]
    public class CommonController : Controller
    {
        public ActionResult DenyAccess()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}