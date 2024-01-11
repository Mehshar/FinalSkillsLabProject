using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Enums;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        Task AddAsync(TrainingModel training, List<int> prerequisitesList);
        Task UpdateAsync(TrainingPrerequisiteViewModel training);
        Task<bool> DeleteAsync(int trainingId);
        Task<TrainingModel> GetAsync(int trainingId);
        Task<IEnumerable<TrainingModel>> GetAllAsync();
        Task<IEnumerable<TrainingModel>> GetAllByUserAsync(int userId);
        Task<IEnumerable<TrainingModel>> GetNotEnrolledTrainingsAsync(int userId);
        Task<IEnumerable<TrainingModel>> GetByDeadlineAsync();
        Task<int> GetNotEnrolledTrainingsCountAsync(int userId);
        Task<IEnumerable<TrainingModel>> GetNotEnrolledTrainingsPagedAsync(int userId, int page, int pageSize);
        Task<TrainingPrerequisiteViewModel> GetWithPrerequisitesAsync(int trainingId);
        Task<bool> IsEnrollmentAsync(int trainingId);
        Task<IEnumerable<UserViewModel>> GetByStatus(int trainingId, EnrollmentStatusEnum status);
    }
}
