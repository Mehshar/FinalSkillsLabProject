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

        public bool Update(EnrollmentModel enrollment)
        {
            return this._enrollmentDAL.Update(enrollment);
        }

        public EnrollmentModel Get(int userId, int trainingId)
        {
            return this._enrollmentDAL.Get(userId, trainingId);
        }

        public IEnumerable<EnrollmentModel> GetAll()
        {
            return this._enrollmentDAL.GetAll();
        }

        public IEnumerable<EnrollmentModel> GetAllByManager(string manager)
        {
            return this._enrollmentDAL.GetAllByManager(manager);
        }
    }
}
