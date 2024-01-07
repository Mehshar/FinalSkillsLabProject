using FinalSkillsLabProject.Authorization;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System;
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

        //public async Task<ActionResult> Index()
        //{
        //    List<TrainingModel> trainingsList = (await _trainingBL.GetNotEnrolledTrainingsAsync(((UserViewModel)Session["CurrentUser"]).UserId)).ToList();
        //    return View(trainingsList);
        //}

        public async Task<ActionResult> Index(int page = 1, int pageSize = 5)
        {
            int userId = ((UserViewModel)Session["CurrentUser"]).UserId;
            int totalTrainings = await _trainingBL.GetNotEnrolledTrainingsCountAsync(userId);
            int totalPages = (int)Math.Ceiling((double)totalTrainings / pageSize);

            List<TrainingModel> trainingsList = (await _trainingBL.GetNotEnrolledTrainingsPagedAsync(userId, page, pageSize)).ToList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalTrainings = totalTrainings;

            if (Request.IsAjaxRequest()) { return Json(new { result = trainingsList }); }

            return View(trainingsList);
        }

        [HttpGet]
        [CustomAuthorization("Admin")]
        public async Task<ActionResult> Create()
        {
            ViewBag.Departments =  (await _departmentBL.GetAllAsync()).ToList();
            ViewBag.Prerequisites = (await _prerequisiteBL.GetAllAsync()).ToList();
            return View();
        }

        [HttpPost]
        [CustomAuthorization("Admin")]
        public async Task<JsonResult> Create(TrainingModel training, List<int> prerequisitesList)
        {
            string result =await _trainingBL.AddAsync(training, prerequisitesList);
            return Json(new { result = result, url = Url.Action("Index", "Training") });
        }

        public async Task<ActionResult> Details(int id)
        {
            TrainingModel training = await _trainingBL.GetAsync((int)id);
            if (training == null) { return View("Error404"); }
            ViewBag.Prerequisites = (await _prerequisiteBL.GetAllByTrainingAsync((int)id)).ToList();
            return View(training);
        }

        [HttpGet]
        [CustomAuthorization("Admin")]
        public async Task<ActionResult> Edit(int id)
        {
            TrainingModel training = await _trainingBL.GetAsync((int)id);
            if (training == null) { return View("Error404"); }
            ViewBag.Departments = (await _departmentBL.GetAllAsync()).Where(x => x.DepartmentId != training.PriorityDepartment).ToList();
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