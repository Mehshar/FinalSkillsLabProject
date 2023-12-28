using System.Collections.Generic;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface ITrainingBL
    {
        Task<string> AddAsync(TrainingModel training, List<int> prerequisitesList);
        Task<string> UpdateAsync(TrainingModel training);
        bool Delete(int trainingId);
        TrainingModel Get(int trainingId);
        IEnumerable<TrainingModel> GetAll();
        IEnumerable<TrainingModel> GetAllByUser(int userId);
        IEnumerable<TrainingModel> GetNotEnrolledTrainings(int userId);
    }
}
