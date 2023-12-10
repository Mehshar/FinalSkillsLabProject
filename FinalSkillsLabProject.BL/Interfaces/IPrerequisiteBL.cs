using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IPrerequisiteBL
    {
        string Add(PrerequisiteModel prerequisite, int trainingId);
        string Update(PrerequisiteModel prerequisite);
        bool Delete(int prerequisiteId);
        IEnumerable<PrerequisiteModel> GetAll();
        IEnumerable<PrerequisiteModel> GetAllByTraining(int trainingId);
    }
}
