using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface ITrainingBL
    {
        Task<string> AddAsync(TrainingModel training, List<int> prerequisitesList);
        Task<string> UpdateAsync(TrainingPrerequisiteViewModel training);
        Task<bool> DeleteAsync(int trainingId);
        Task<TrainingModel> GetAsync(int trainingId);
        Task<IEnumerable<TrainingModel>> GetAllAsync();
        Task<IEnumerable<TrainingModel>> GetAllByUserAsync(int userId);
        Task<IEnumerable<TrainingModel>> GetNotEnrolledTrainingsAsync(int userId);
        Task<IEnumerable<TrainingModel>> GetByDeadlineAsync();
        Task<int> GetNotEnrolledTrainingsCountAsync(int userId);
        Task<IEnumerable<TrainingModel>> GetNotEnrolledTrainingsPagedAsync(int userId, int page, int pageSize);
        Task<TrainingPrerequisiteViewModel> GetWithPrerequisitesAsync(int trainingId);
    }
}
