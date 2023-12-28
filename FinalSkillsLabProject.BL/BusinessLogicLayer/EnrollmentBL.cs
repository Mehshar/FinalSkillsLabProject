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
    }
}
