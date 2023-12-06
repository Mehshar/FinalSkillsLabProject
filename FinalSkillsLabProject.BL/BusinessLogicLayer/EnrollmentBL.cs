using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool Add(EnrollmentModel enrollment)
        {
            return this._enrollmentDAL.Add(enrollment);
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
