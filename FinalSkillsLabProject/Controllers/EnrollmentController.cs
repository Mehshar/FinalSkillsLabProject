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

namespace FinalSkillsLabProject.Controllers
{
    public class EnrollmentController : Controller
    {
        private static string ApiKey = "AIzaSyA57WmkAd63zaf0GSQeYwwjfMn2ixp-ebM";
        private static string Bucket = "skillslabproject-4b1a2.appspot.com";
        private static string AuthEmail = "khatidja.mauraknah@ceridian.com";
        private static string AuthPassword = "Mehshar@Ceridian786";

        private readonly ITrainingBL _trainingBL;
        private readonly IPrerequisiteBL _prerequisiteBL;
        private readonly IEnrollmentBL _enrollmentBL;

        public EnrollmentController(ITrainingBL trainingBL, IPrerequisiteBL prerequisiteBL, IEnrollmentBL enrollmentBL)
        {
            _trainingBL = trainingBL;
            _prerequisiteBL = prerequisiteBL;
            _enrollmentBL = enrollmentBL;
        }

        [HttpGet]
        public ActionResult Enroll(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            TrainingModel training = _trainingBL.Get((int)id);
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

                bool isSuccess = _enrollmentBL.Add(enrollment, prerequisiteMaterialsList);
                return Json(new { result = isSuccess, url = Url.Action("Index", "Training") });
            }
            catch (Exception ex)
            {
                return Json(new { result = false });
            }
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string folder)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Bucket,
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
    }
}