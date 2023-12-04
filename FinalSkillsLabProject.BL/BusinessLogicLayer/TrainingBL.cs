using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Exceptions;
using FinalSkillsLabProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class TrainingBL : ITrainingBL
    {
        private readonly ITrainingDAL _trainingDAL;

        public TrainingBL(ITrainingDAL trainingDAL)
        {
            this._trainingDAL = trainingDAL;
        }

        public string Add(TrainingModel training, List<int> prerequisitesList)
        {
            try
            {
                TrainingModel trainingModel = GetAll().FirstOrDefault(x => x.TrainingName.Equals(training.TrainingName));
                CheckInsertUpdateDuplicate(trainingModel);
                this._trainingDAL.Add(training, prerequisitesList);
                return "Training created successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(TrainingModel training)
        {
            try
            {
                TrainingModel trainingModel = GetAll()
                    .Where(x => x.TrainingId != training.TrainingId)
                    .FirstOrDefault(x => x.TrainingName.Equals(training.TrainingName));
                CheckInsertUpdateDuplicate(trainingModel);
                this._trainingDAL.Update(training);
                return "Training updated successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool Delete(int trainingId)
        {
            return this._trainingDAL.Delete(trainingId);
        }

        public TrainingModel Get(int trainingId)
        {
            return this._trainingDAL.Get(trainingId);
        }

        public IEnumerable<TrainingModel> GetAll()
        {
            return this._trainingDAL.GetAll();
        }

        public IEnumerable<TrainingModel> GetAllByUser(int userId)
        {
            return this._trainingDAL.GetAllByUser(userId);
        }

        private void CheckInsertUpdateDuplicate(TrainingModel training)
        {
            if (training != null)
            {
                string message = "Training name already exists!";
                throw new DuplicationException(message);
            }
        }
    }
}
