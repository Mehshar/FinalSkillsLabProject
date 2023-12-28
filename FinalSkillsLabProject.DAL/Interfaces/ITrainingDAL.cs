using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        Task AddAsync(TrainingModel training, List<int> prerequisitesList);
        Task UpdateAsync(TrainingModel training);
        bool Delete(int trainingId);
        TrainingModel Get(int trainingId);
        IEnumerable<TrainingModel> GetAll();
        IEnumerable<TrainingModel> GetAllByUser(int userId);
        IEnumerable<TrainingModel> GetNotEnrolledTrainings(int userId);
    }
}
