using FinalSkillsLabProject.Authorization;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinalSkillsLabProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IEnrollmentBL _enrollmentBL;

        public UserController(IEnrollmentBL enrollmentBL)
        {
            _enrollmentBL = enrollmentBL;
        }

        [HttpGet]
        public ActionResult Index()
        {
            UserViewModel user = ((UserViewModel)Session["CurrentUser"]);
            return View(user);
        }

        [HttpGet]
        //[CustomAuthorization("Employee")]
        public async Task<ActionResult> Enrollments()
        {
            List<EnrollmentViewModel> enrollmentsList = (await _enrollmentBL.GetAllByUser(((UserViewModel)Session["CurrentUser"]).UserId)).ToList();
            return View(enrollmentsList);
        }
    }
}