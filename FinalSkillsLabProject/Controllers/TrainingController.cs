﻿using FinalSkillsLabProject.Authorization;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FinalSkillsLabProject.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingBL _trainingBL;
        private readonly IDepartmentBL _departmentBL;
        private readonly IPrerequisiteBL _prerequisiteBL;

        public TrainingController(ITrainingBL trainingBL, IDepartmentBL departmentBL, IPrerequisiteBL prerequisiteBL)
        {
            _trainingBL = trainingBL;
            _departmentBL = departmentBL;
            _prerequisiteBL = prerequisiteBL;
        }

        public ActionResult Index()
        {
            List<TrainingModel> trainingsList = _trainingBL.GetNotEnrolledTrainings((int)Session["CurrentUserId"]).ToList();
            return View(trainingsList);
        }

        [HttpGet]
        [CustomAuthorization("Admin")]
        public ActionResult Create()
        {
            ViewBag.Departments = _departmentBL.GetAll().ToList();
            ViewBag.Prerequisites = _prerequisiteBL.GetAll().ToList();
            return View();
        }

        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<JsonResult> Create(TrainingModel training, List<int> prerequisitesList)
        {
            string result =await _trainingBL.AddAsync(training, prerequisitesList);
            return Json(new { result = result, url = Url.Action("Index", "Training") });
        }

        public ActionResult Details(int? id)
        {
            if (id == null) { return View("Error404"); }
            TrainingModel training = _trainingBL.Get((int)id);
            if (training == null) { return View("Error404"); }
            ViewBag.Prerequisites = _prerequisiteBL.GetAllByTraining((int)id).ToList();
            return View(training);
        }

        [HttpGet]
        [CustomAuthorization("Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null) { return View("Error404"); }
            TrainingModel training = _trainingBL.Get((int)id);
            if (training == null) { return View("Error404"); }
            ViewBag.Departments = _departmentBL.GetAll().Where(x => x.DepartmentId != training.PriorityDepartment).ToList();
            return View(training);
        }

        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<JsonResult> SaveEdit(TrainingModel training)
        {
            string result = await _trainingBL.UpdateAsync(training);
            return Json(new { result = result, url = Url.Action("Index", "Training") });
        }
    }
}