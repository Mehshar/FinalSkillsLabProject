using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class EnrollmentBL : IEnrollmentBL
    {
        private readonly IEnrollmentDAL _enrollmentDAL;

        public EnrollmentBL(IEnrollmentDAL enrollmentDAL)
        {
            this._enrollmentDAL = enrollmentDAL;
        }

        public async Task<bool> AddAsync(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList)
        {
            return await this._enrollmentDAL.AddAsync(enrollment, prerequisiteMaterialsList);
        }

        public async Task<bool> UpdateAsync(int enrollmentId, bool isApproved, string declineReason)
        {
            return await this._enrollmentDAL.UpdateAsync(enrollmentId, isApproved, declineReason);
        }

        public EnrollmentModel Get(int userId, int trainingId)
        {
            return this._enrollmentDAL.Get(userId, trainingId);
        }

        public IEnumerable<EnrollmentViewModel> GetAll()
        {
            return this._enrollmentDAL.GetAll();
        }

        public IEnumerable<EnrollmentViewModel> GetAllByManagerTraining(int managerId, int trainingId)
        {
            return this._enrollmentDAL.GetAllByManagerTraining(managerId, trainingId);
        }

        public IEnumerable<EnrollmentViewModel> GetAllByManager(int managerId)
        {
            return this._enrollmentDAL.GetAllByManager(managerId);
        }

        public IEnumerable<PrerequisiteMaterialViewModel> GetPrerequisiteMaterialsByEnrollment(int enrollmentId)
        {
            return this._enrollmentDAL.GetPrerequisiteMaterialsByEnrollment(enrollmentId);
        }

        public UserEnrollmentViewModel GetUserByEnrollment(int enrollmentId)
        {
            return this._enrollmentDAL.GetUserByEnrollment(enrollmentId);
        }

        public string GetDeclineReasonByEnrollment(int enrollmentId)
        {
            return this._enrollmentDAL.GetDeclineReasonByEnrollment(enrollmentId);
        }
    }
}
