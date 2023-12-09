using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IEnrollmentDAL
    {
        bool Add(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList);
        bool Update(EnrollmentModel enrollment);
        //void Delete(EnrollmentModel model);
        EnrollmentModel Get(int userId, int trainingId);
        IEnumerable<EnrollmentModel> GetAll();
        IEnumerable<EnrollmentModel> GetAllByManager(string manager);
    }
}
