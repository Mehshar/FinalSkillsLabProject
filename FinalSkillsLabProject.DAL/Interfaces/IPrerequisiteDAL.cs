using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IPrerequisiteDAL
    {
        Task AddAsync(PrerequisiteModel prerequisite, int trainingId);
        Task UpdateAsync(PrerequisiteModel prerequisite);
        Task<bool> DeleteAsync(int prerequisiteId);
        Task<IEnumerable<PrerequisiteModel>> GetAllAsync();
        Task<IEnumerable<PrerequisiteModel>> GetAllByTrainingAsync(int trainingId);
    }
}
