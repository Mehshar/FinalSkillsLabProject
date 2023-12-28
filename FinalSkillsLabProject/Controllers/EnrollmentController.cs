using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using FinalSkillsLabProject.Authorization;
using FinalSkillsLabProject.Common.Enums;

namespace FinalSkillsLabProject.Controllers
{
    public class EnrollmentController : Controller
    {
        private static string _apiKey = ConfigurationManager.AppSettings["ApiKey"];
        private static string _bucket = ConfigurationManager.AppSettings["Bucket"];
        private static string _authEmail = ConfigurationManager.AppSettings["AuthEmail"];
        private static string _authPassword = ConfigurationManager.AppSettings["AuthPassword"];

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
        public ActionResult Enroll(int? id)
        {
            if (id == null) { return View("Error404"); }
            TrainingModel training = _trainingBL.Get((int)id);
            if (training == null) { return View("Error404"); }
            ViewBag.Prerequisites = _prerequisiteBL.GetAllByTraining((int)id);
            return View(training);
        }

        [HttpPost]
        public async Task<ActionResult> Enroll(int trainingId, List<int> prerequisiteIds)
        {
            try
            {
                PrerequisiteMaterialModel prerequisiteMaterial;
                List<PrerequisiteMaterialModel> prerequisiteMaterialsList = new List<PrerequisiteMaterialModel>();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var allowedTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
                        if (!allowedTypes.Contains(file.ContentType))
                        {
                            return Json(new { result = false, error = "Please select a JPEG, PNG or PDF file" });
                        }

                        string fileName = Path.GetFileName(file.FileName);
                        string folder = "Prerequisite_" + prerequisiteIds[i];
                        string link = await UploadAsync(file.InputStream, fileName, folder);

                        prerequisiteMaterial = new PrerequisiteMaterialModel()
                        {
                            PrerequisiteId = prerequisiteIds[i],
                            PrerequisiteMaterialURL = link
                        };
                        prerequisiteMaterialsList.Add(prerequisiteMaterial);
                    }

                    else
                    {
                        return Json(new { result = false, error = "Please select a file" });
                    }
                }

                EnrollmentModel enrollment = new EnrollmentModel()
                {
                    UserId = (int)Session["CurrentUserId"],
                    TrainingId = trainingId
                };

                bool isSuccess = await _enrollmentBL.AddAsync(enrollment, prerequisiteMaterialsList);
                return Json(new { result = isSuccess, url = Url.Action("Index", "Training") });
            }
            catch (Exception)
            {
                return Json(new { result = false });
            }
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string folder)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(_authEmail, _authPassword);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    _bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(Session["CurrentUsername"].ToString())
                    .Child(folder)
                    .Child(fileName)
                    .PutAsync(stream, cancellation.Token);
                string link = await task;
                return link;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorization("Manager,Admin")]
        public ActionResult EmployeeEnrollments()
        {
            List<EnrollmentViewModel> employeeEnrollmentsList = null;
            
            if (Session["CurrentRole"].ToString().Equals("Admin"))
            {
                //employeeEnrollmentsList = _enrollmentBL.GetAll().Where(x => x.EnrollmentStatus != EnrollmentStatusEnum.Selected.ToString()).ToList();
                employeeEnrollmentsList = _enrollmentBL.GetAll().ToList();
            }

            else if (Session["CurrentRole"].ToString().Equals("Manager"))
            {
                employeeEnrollmentsList = _enrollmentBL.GetAllByManager((int)Session["CurrentUserId"]).Where(x => x.EnrollmentStatus != EnrollmentStatusEnum.Selected.ToString()).ToList();
            }
            return View(employeeEnrollmentsList);
        }

        [CustomAuthorization("Manager,Admin")]
        public ActionResult TrainingEnrollmentsMaterials(int id)
        {
            List<PrerequisiteMaterialViewModel> prerequisiteMaterialsList = _enrollmentBL.GetPrerequisiteMaterialsByEnrollment(id).ToList();
            return PartialView(prerequisiteMaterialsList);
        }

        [CustomAuthorization("Manager,Admin")]
        public async Task<JsonResult> ManageEnrollment(int enrollmentId, bool isApproved, string declineReason)
        {
            bool result = await _enrollmentBL.UpdateAsync(enrollmentId, isApproved, declineReason);
            UserEnrollmentViewModel user = _enrollmentBL.GetUserByEnrollment(enrollmentId);
            if (result)
            {
                UserViewModel requestHandler = (UserViewModel)Session["CurrentUser"];
                string requestHandlerName = $"{requestHandler.FirstName} {requestHandler.LastName}";
                _emailNotificationBL.SendEmail(isApproved, user.Email, user.Username, user.TrainingName, requestHandlerName, requestHandler.Role.RoleName.ToString().ToLower(), requestHandler.Email, declineReason, user.ManagerEmail);
            }
            return Json(new { result = result });
        }

        [HttpGet]
        [CustomAuthorization("Manager,Admin")]
        public JsonResult GetDeclineReasonByEnrollment(int enrollmentId)
        {
            string declineReason = _enrollmentBL.GetDeclineReasonByEnrollment(enrollmentId);
            return Json(new { result = declineReason }, JsonRequestBehavior.AllowGet);
        }
    }
}