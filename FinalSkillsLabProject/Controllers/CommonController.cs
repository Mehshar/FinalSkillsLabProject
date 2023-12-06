using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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