using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IEnrollmentBL
    {
        bool Add(EnrollmentModel enrollment);
        bool Update(EnrollmentModel enrollment);
        //void Delete(EnrollmentModel model);
        EnrollmentModel Get(int userId, int trainingId);
        IEnumerable<EnrollmentModel> GetAll();
        IEnumerable<EnrollmentModel> GetAllByManager(string manager);
    }
}
