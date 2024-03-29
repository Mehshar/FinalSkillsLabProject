﻿using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FinalSkillsLabProject.Authorization;

namespace FinalSkillsLabProject.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ITrainingBL _trainingBL;
        private readonly IPrerequisiteBL _prerequisiteBL;
        private readonly IEnrollmentBL _enrollmentBL;
        private readonly IEmailNotificationBL _emailNotificationBL;

        public EnrollmentController(ITrainingBL trainingBL, IPrerequisiteBL prerequisiteBL, IEnrollmentBL enrollmentBL, IEmailNotificationBL emailNotificationBL)
        {
            _trainingBL = trainingBL;
            _prerequisiteBL = prerequisiteBL;
            _enrollmentBL = enrollmentBL;
            _emailNotificationBL = emailNotificationBL;
        }

        [HttpGet]
        public async Task<ActionResult> Enroll(int? id)
        {
            TrainingModel training = await _trainingBL.GetAsync((int)id);
            if (training == null) { return View("Error404"); }
            if (training.IsDeleted || training.Deadline < DateTime.Now) { return View("Error"); }
            ViewBag.Prerequisites = await _prerequisiteBL.GetAllByTrainingAsync((int)id);
            return View(training);
        }

        [HttpPost]
        public async Task<ActionResult> Enroll(int trainingId, List<int> prerequisiteIds)
        {
            UserViewModel user = (UserViewModel)Session["CurrentUser"];
            EnrollmentResult enrollmentResult = await _enrollmentBL.Enroll(trainingId, prerequisiteIds, Request.Files, user);

            return enrollmentResult.IsSuccess ? Json(new { result = true, url = Url.Action("Index", "Training") })
                : Json(new { result = false, message = enrollmentResult.ErrorMessage });
        }

        [HttpGet]
        [CustomAuthorization("Manager,Admin")]
        public async Task<ActionResult> EmployeeEnrollments()
        {           
            string currentRole = ((UserViewModel)Session["CurrentUser"]).Role.RoleName.ToString();
            int currentUserId = ((UserViewModel)Session["CurrentUser"]).UserId;
            ViewBag.Trainings = (await _trainingBL.GetAllAsync()).ToList();

            List<EnrollmentViewModel> employeeEnrollmentsList = (await _enrollmentBL.GetByRole(currentRole, currentUserId)).ToList();
            return View(employeeEnrollmentsList);
        }

        public async Task<ActionResult> TrainingEnrollmentsMaterials(int id)
        {
            List<PrerequisiteMaterialViewModel> prerequisiteMaterialsList = (await _enrollmentBL.GetPrerequisiteMaterialsByEnrollmentAsync(id)).ToList();
            ViewBag.EnrollmentId = id;
            return PartialView(prerequisiteMaterialsList);
        }

        [CustomAuthorization("Manager,Admin")]
        public async Task<JsonResult> ManageEnrollment(int enrollmentId, bool isApproved, string declineReason)
        {
            bool result = await _enrollmentBL.UpdateAsync(enrollmentId, isApproved, declineReason);
            UserEnrollmentViewModel user = await _enrollmentBL.GetUserByEnrollmentAsync(enrollmentId);
            if (result)
            {
                UserViewModel requestHandler = (UserViewModel)Session["CurrentUser"];
                string requestHandlerName = $"{requestHandler.FirstName} {requestHandler.LastName}";
                await _emailNotificationBL.SendApprovalRejectionEmailAsync(isApproved, user.Email, user.Username, user.TrainingName, requestHandlerName, requestHandler.Role.RoleName.ToString().ToLower(), requestHandler.Email, declineReason, user.ManagerEmail);
            }
            return Json(new { result = result });
        }

        [HttpGet]
        public async Task<JsonResult> GetDeclineReasonByEnrollment(int enrollmentId)
        {
            string declineReason = await _enrollmentBL.GetDeclineReasonByEnrollmentAsync(enrollmentId);
            return Json(new { result = declineReason }, JsonRequestBehavior.AllowGet);
        }
    }
}