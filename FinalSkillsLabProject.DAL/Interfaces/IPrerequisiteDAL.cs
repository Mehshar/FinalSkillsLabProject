using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IPrerequisiteDAL
    {
        void Add(PrerequisiteModel prerequisite, int trainingId);
        void Update(PrerequisiteModel prerequisite);
        bool Delete(int prerequisiteId);
        IEnumerable<PrerequisiteModel> GetAll();
        IEnumerable<PrerequisiteModel> GetAllByTraining(int trainingId);
    }
}
