using System.Collections.Generic;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        void Add(TrainingModel training, List<int> prerequisitesList);
        void Update(TrainingModel training);
        bool Delete(int trainingId);
        TrainingModel Get(int trainingId);
        IEnumerable<TrainingModel> GetAll();
        IEnumerable<TrainingModel> GetAllByUser(int userId);
        IEnumerable<TrainingModel> GetNotEnrolledTrainings(int userId);
    }
}
