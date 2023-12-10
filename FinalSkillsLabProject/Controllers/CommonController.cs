using System.Web.Mvc;

namespace FinalSkillsLabProject.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult DenyAccess()
        {
            return View();
        }
    }
}