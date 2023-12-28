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
        EnrollmentModel Get(int userId, int trainingId);
        IEnumerable<EnrollmentViewModel> GetAll();
        IEnumerable<EnrollmentViewModel> GetAllByManagerTraining(int managerId, int trainingId);
        IEnumerable<EnrollmentViewModel> GetAllByManager(int managerId);
        IEnumerable<PrerequisiteMaterialViewModel> GetPrerequisiteMaterialsByEnrollment(int enrollmentId);
        UserEnrollmentViewModel GetUserByEnrollment(int enrollmentId);
        string GetDeclineReasonByEnrollment(int enrollmentId);
    }
}
