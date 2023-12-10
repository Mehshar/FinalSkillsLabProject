using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IEnrollmentBL
    {
        bool Add(EnrollmentModel enrollment, List<PrerequisiteMaterialModel> prerequisiteMaterialsList);
        bool Update(EnrollmentModel enrollment);
        //void Delete(EnrollmentModel model);
        EnrollmentModel Get(int userId, int trainingId);
        IEnumerable<EnrollmentModel> GetAll();
        IEnumerable<EnrollmentModel> GetAllByManager(string manager);
    }
}
