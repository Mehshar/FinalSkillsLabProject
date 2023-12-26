using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface ITrainingBL
    {
        string Add(TrainingModel training, List<int> prerequisitesList);
        string Update(TrainingModel training);
        bool Delete(int trainingId);
        TrainingModel Get(int trainingId);
        IEnumerable<TrainingModel> GetAll();
        IEnumerable<TrainingModel> GetAllByUser(int userId);
        IEnumerable<TrainingModel> GetNotEnrolledTrainings(int userId);
    }
}
