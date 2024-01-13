using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;
using FinalSkillsLabProject.DAL.DataAccessLayer;
using System.Linq;
using Firebase.Auth;
using Firebase.Storage;
using System.IO;
using System.Threading;
using System.Configuration;
using Azure.Core;
using System.Security.Policy;
using System.Web;
using System;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class EnrollmentBL : IEnrollmentBL
    {
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly ITrainingDAL _trainingDAL;
        private readonly IEmailNotificationBL _emailNotificationBL;
        private readonly ITrainingBL _trainingBL;

        private static string _apiKey = ConfigurationManager.AppSettings["ApiKey"];
        private static string _bucket = ConfigurationManager.AppSettings["Bucket"];
        private static string _authEmail = ConfigurationManager.AppSettings["AuthEmail"];
        private static string _authPassword = ConfigurationManager.AppSettings["AuthPassword"];

        public EnrollmentBL(IEnrollmentDAL enrollmentDAL, ITrainingDAL trainingDAL, IEmailNotificationBL emailNotificationBL, ITrainingBL trainingBL)
        {
            this._enrollmentDAL = enrollmentDAL;
            _trainingDAL = trainingDAL;
            _emailNotificationBL = emailNotificationBL;
            _trainingBL = trainingBL;
        }

        public async Task<bool> AddAsync(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList)
        {
            return await this._enrollmentDAL.AddAsync(enrollment, prerequisiteMaterialsList);
        }

        public async Task<bool> UpdateAsync(int enrollmentId, bool isApproved, string declineReason)
        {
            return await this._enrollmentDAL.UpdateAsync(enrollmentId, isApproved, declineReason);
        }

        public async Task<EnrollmentModel> GetAsync(int userId, int trainingId)
        {
            return await this._enrollmentDAL.GetAsync(userId, trainingId);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllAsync()
        {
            return await this._enrollmentDAL.GetAllAsync();
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllByManagerTrainingAsync(int managerId, int trainingId)
        {
            return await this._enrollmentDAL.GetAllByManagerTrainingAsync(managerId, trainingId);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllByManagerAsync(int managerId)
        {
            return await this._enrollmentDAL.GetAllByManagerAsync(managerId);
        }

        public async Task<IEnumerable<PrerequisiteMaterialViewModel>> GetPrerequisiteMaterialsByEnrollmentAsync(int enrollmentId)
        {
            return await this._enrollmentDAL.GetPrerequisiteMaterialsByEnrollmentAsync(enrollmentId);
        }

        public async Task<UserEnrollmentViewModel> GetUserByEnrollmentAsync(int enrollmentId)
        {
            return await this._enrollmentDAL.GetUserByEnrollmentAsync(enrollmentId);
        }

        public async Task<string> GetDeclineReasonByEnrollmentAsync(int enrollmentId)
        {
            return await this._enrollmentDAL.GetDeclineReasonByEnrollmentAsync(enrollmentId);
        }

        public async Task<IEnumerable<EnrollmentSelectionViewModel>> UpdateAfterDeadlineAsync(int trainingId, string declineReason)
        {
            return await this._enrollmentDAL.UpdateAfterDeadlineAsync(trainingId, declineReason);
        }

        public async Task<IEnumerable<EnrollmentSelectionViewModel>> GetSelectedEnrollmentsByTrainingAsync(int trainingId)
        {
            return await this._enrollmentDAL.GetSelectedEnrollmentsByTrainingAsync(trainingId);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllByUser(int userId)
        {
            return await this._enrollmentDAL.GetAllByUser(userId);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetByRole(string currentRole, int currentUserId)
        {
            List<EnrollmentViewModel> employeeEnrollmentsList = null;
            if (currentRole.Equals("Admin"))
            {
                employeeEnrollmentsList = (await GetAllAsync()).ToList();
            }

            else if (currentRole.Equals("Manager"))
            {
                employeeEnrollmentsList = (await GetAllByManagerAsync(currentUserId)).ToList();
            }
            return employeeEnrollmentsList;
        }

        public async Task<EnrollmentResult> Enroll(int trainingId, List<int> prerequisiteIds, HttpFileCollectionBase files, UserViewModel user)
        {
            try
            {
                PrerequisiteMaterialModel prerequisiteMaterial;
                List<PrerequisiteMaterialModel> prerequisiteMaterialsList = new List<PrerequisiteMaterialModel>();

                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var allowedTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
                        if (!allowedTypes.Contains(file.ContentType))
                        {
                            return new EnrollmentResult { IsSuccess = false, ErrorMessage = "Please select a JPEG, PNG or PDF file" };
                        }

                        string fileName = Path.GetFileName(file.FileName);
                        string folder = "Prerequisite_" + prerequisiteIds[i];
                        string link = await UploadAsync(file.InputStream, fileName, folder, user.Username);

                        prerequisiteMaterial = new PrerequisiteMaterialModel()
                        {
                            PrerequisiteId = prerequisiteIds[i],
                            PrerequisiteMaterialURL = link
                        };
                        prerequisiteMaterialsList.Add(prerequisiteMaterial);
                    }

                    else
                    {
                        return new EnrollmentResult { IsSuccess = false, ErrorMessage = "Please select a file" };
                    }
                }

                EnrollmentModel enrollment = new EnrollmentModel()
                {
                    UserId = user.UserId,
                    TrainingId = trainingId
                };

                bool isSuccess = await AddAsync(enrollment, prerequisiteMaterialsList);
                if (isSuccess) { await _emailNotificationBL.SendEnrollmentEmail(user, await _trainingBL.GetAsync(trainingId)); }
                return new EnrollmentResult { IsSuccess = isSuccess };
            }
            catch (Exception)
            {
                return new EnrollmentResult { IsSuccess = false };
            }
        }

        private async Task<string> UploadAsync(Stream stream, string fileName, string folder, string username)
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
                .Child(username)
                .Child(folder)
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);
            string link = await task;
            return link;
        }

        public async Task EnrollmentAutomaticProcessing()
        {
            List<TrainingModel> trainingsList = (await _trainingDAL.GetByDeadlineAsync()).ToList();
            List<EnrollmentSelectionViewModel> selectedEnrollmentsList;
            List<EnrollmentSelectionViewModel> declinedEnrollmentsList;
            string declineReason = "Training capacity reached";
            int delayMilliseconds = 3000;
            foreach (TrainingModel training in trainingsList)
            {
                declinedEnrollmentsList = (await UpdateAfterDeadlineAsync(training.TrainingId, declineReason)).ToList();
                foreach (EnrollmentSelectionViewModel declinedEnrollment in declinedEnrollmentsList)
                {
                    _emailNotificationBL.SendSelectionRejectionEmail(false, declinedEnrollment);
                    await Task.Delay(delayMilliseconds); // Introduce a delay
                }

                selectedEnrollmentsList = (await GetSelectedEnrollmentsByTrainingAsync(training.TrainingId)).ToList();
                foreach (EnrollmentSelectionViewModel selectedEnrollment in selectedEnrollmentsList)
                {
                    _emailNotificationBL.SendSelectionRejectionEmail(true, selectedEnrollment);
                    await Task.Delay(delayMilliseconds);
                }
            }
        }
    }
}
