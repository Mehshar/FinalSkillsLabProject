using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IEnrollmentBL
    {
        Task<bool> AddAsync(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList);
        Task<bool> UpdateAsync(int enrollmentId, bool isApproved, string declineReason);
        //void Delete(EnrollmentModel model);
        Task<EnrollmentModel> GetAsync(int userId, int trainingId);
        Task<IEnumerable<EnrollmentViewModel>> GetAllAsync();
        Task<IEnumerable<EnrollmentViewModel>> GetAllByManagerTrainingAsync(int managerId, int trainingId);
        Task<IEnumerable<EnrollmentViewModel>> GetAllByManagerAsync(int managerId);
        Task<IEnumerable<PrerequisiteMaterialViewModel>> GetPrerequisiteMaterialsByEnrollmentAsync(int enrollmentId);
        Task<UserEnrollmentViewModel> GetUserByEnrollmentAsync(int enrollmentId);
        Task<string> GetDeclineReasonByEnrollmentAsync(int enrollmentId);
        Task<IEnumerable<EnrollmentSelectionViewModel>> UpdateAfterDeadlineAsync(int trainingId, string declineReason);
        Task<IEnumerable<EnrollmentViewModel>> GetAllByUser(int userId);
        Task EnrollmentAutomaticProcessing();
        Task<EnrollmentResult> Enroll(int trainingId, List<int> prerequisiteIds, HttpFileCollectionBase files, UserViewModel user);
    }
}
