using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
