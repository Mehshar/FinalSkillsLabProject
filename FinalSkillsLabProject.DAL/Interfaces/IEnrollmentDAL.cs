using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IEnrollmentDAL
    {
        bool Add(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList);
        bool Update(int enrollmentId, bool isApproved, string declineReason);
        EnrollmentModel Get(int userId, int trainingId);
        IEnumerable<EnrollmentViewModel> GetAll();
        IEnumerable<EnrollmentViewModel> GetAllByManagerTraining(int managerId, int trainingId);
        IEnumerable<EnrollmentViewModel> GetAllByManager(int managerId);
        IEnumerable<PrerequisiteMaterialViewModel> GetPrerequisiteMaterialsByEnrollment(int enrollmentId);
        UserEnrollmentViewModel GetUserByEnrollment(int enrollmentId);
        string GetDeclineReasonByEnrollment(int enrollmentId);
    }
}
