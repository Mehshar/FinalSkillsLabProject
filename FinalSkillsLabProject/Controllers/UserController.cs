using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FinalSkillsLabProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IEnrollmentBL _enrollmentBL;
        private readonly ITrainingBL _trainingBL;

        public UserController(IEnrollmentBL enrollmentBL, ITrainingBL trainingBL)
        {
            _enrollmentBL = enrollmentBL;
            _trainingBL = trainingBL;
        }

        [HttpGet]
        public ActionResult Index()
        {
            UserViewModel user = ((UserViewModel)Session["CurrentUser"]);
            return View(user);
        }

        [HttpGet]
        public async Task<ActionResult> Enrollments()
        {
            ViewBag.Trainings = (await _trainingBL.GetAllAsync()).ToList();
            List<EnrollmentViewModel> enrollmentsList = (await _enrollmentBL.GetAllByUser(((UserViewModel)Session["CurrentUser"]).UserId)).ToList();
            return View(enrollmentsList);
        }
    }
}