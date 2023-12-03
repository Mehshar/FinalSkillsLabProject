using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinalSkillsLabProject.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingBL _trainingBL;
        private readonly IDepartmentBL _departmentBL;

        public TrainingController(ITrainingBL trainingBL, IDepartmentBL departmentBL)
        {
            _trainingBL = trainingBL;
            _departmentBL = departmentBL;
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TrainingModel training = _trainingBL.Get((int)id);
            ViewBag.Departments = _departmentBL.GetAll().Where(x => x.DepartmentId != training.PriorityDepartment).ToList();
            return View(training);
        }

        [HttpPost]
        public JsonResult SaveEdit(TrainingModel training)
        {
            string result = _trainingBL.Update(training);
            return Json(new { result = result, url = Url.Action("GetAll", "Training") });
        }

        public ActionResult GetAll()
        {
            List<TrainingModel> trainingsList = _trainingBL.GetAll().ToList();
            ViewBag.Trainings = trainingsList;
            return View();
        }
    }
}