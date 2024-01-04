using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;
using FinalSkillsLabProject.DAL.DataAccessLayer;
using System.Linq;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class EnrollmentBL : IEnrollmentBL
    {
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly ITrainingDAL _trainingDAL;
        private readonly IEmailNotificationBL _emailNotificationBL;

        public EnrollmentBL(IEnrollmentDAL enrollmentDAL, ITrainingDAL trainingDAL, IEmailNotificationBL emailNotificationBL)
        {
            this._enrollmentDAL = enrollmentDAL;
            _trainingDAL = trainingDAL;
            _emailNotificationBL = emailNotificationBL;
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
