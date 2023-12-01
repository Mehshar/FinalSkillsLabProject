using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalSkillsLabProject.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingBL _trainingBL;

        public TrainingController(ITrainingBL trainingBL)
        {
            _trainingBL = trainingBL;
        }

        public ActionResult GetAll()
        {
            List<TrainingModel> trainingsList = _trainingBL.GetAll().ToList();
            ViewBag.Trainings = trainingsList;
            return View();
        }
    }
}