using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IPrerequisiteBL
    {
        Task<string> AddAsync(PrerequisiteModel prerequisite, int trainingId);
        Task<string> UpdateAsync(PrerequisiteModel prerequisite);
        Task<bool> DeleteAsync(int prerequisiteId);
        Task<IEnumerable<PrerequisiteModel>> GetAllAsync();
        Task<IEnumerable<PrerequisiteModel>> GetAllByTrainingAsync(int trainingId);
    }
}
