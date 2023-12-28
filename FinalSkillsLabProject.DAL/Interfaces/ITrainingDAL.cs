using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        Task AddAsync(TrainingModel training, List<int> prerequisitesList);
        Task UpdateAsync(TrainingModel training);
        Task<bool> DeleteAsync(int trainingId);
        Task<TrainingModel> GetAsync(int trainingId);
        Task<IEnumerable<TrainingModel>> GetAllAsync();
        Task<IEnumerable<TrainingModel>> GetAllByUserAsync(int userId);
        Task<IEnumerable<TrainingModel>> GetNotEnrolledTrainingsAsync(int userId);
    }
}
