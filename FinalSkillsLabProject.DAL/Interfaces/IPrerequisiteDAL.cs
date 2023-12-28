using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IPrerequisiteDAL
    {
        Task AddAsync(PrerequisiteModel prerequisite, int trainingId);
        Task UpdateAsync(PrerequisiteModel prerequisite);
        bool Delete(int prerequisiteId);
        IEnumerable<PrerequisiteModel> GetAll();
        IEnumerable<PrerequisiteModel> GetAllByTraining(int trainingId);
    }
}
