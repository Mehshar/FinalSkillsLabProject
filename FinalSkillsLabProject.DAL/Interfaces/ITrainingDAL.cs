using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface ITrainingDAL
    {
        void Add(TrainingModel training);
        void Update(TrainingModel training);
        bool Delete(int trainingId);
        TrainingModel Get(int trainingId);
        IEnumerable<TrainingModel> GetAll();
        IEnumerable<TrainingModel> GetAllByUser(int userId);
    }
}
