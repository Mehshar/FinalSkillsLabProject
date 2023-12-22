using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class EnrollmentBL : IEnrollmentBL
    {
        private readonly IEnrollmentDAL _enrollmentDAL;

        public EnrollmentBL(IEnrollmentDAL enrollmentDAL)
        {
            this._enrollmentDAL = enrollmentDAL;
        }

        public bool Add(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList)
        {
            return this._enrollmentDAL.Add(enrollment, prerequisiteMaterialsList);
        }

        public bool Update(int enrollmentId, bool isApproved, string declineReason)
        {
            return this._enrollmentDAL.Update(enrollmentId, isApproved, declineReason);
        }

        public EnrollmentModel Get(int userId, int trainingId)
        {
            return this._enrollmentDAL.Get(userId, trainingId);
        }

        public IEnumerable<EnrollmentModel> GetAll()
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
