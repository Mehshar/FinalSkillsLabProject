using System.Web.Mvc;

namespace FinalSkillsLabProject.Controllers
{
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