using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Exceptions;
using FinalSkillsLabProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class TrainingBL : ITrainingBL
    {
        private readonly ITrainingDAL _trainingDAL;

        public TrainingBL(ITrainingDAL trainingDAL)
        {
            this._trainingDAL = trainingDAL;
        }

        public async Task<string> AddAsync(TrainingModel training, List<int> prerequisitesList)
        {
            try
            {
                TrainingModel trainingModel = GetAll().FirstOrDefault(x => x.TrainingName.Equals(training.TrainingName));
                CheckInsertUpdateDuplicate(trainingModel);
                await this._trainingDAL.AddAsync(training, prerequisitesList);
                return "Training created successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateAsync(TrainingModel training)
        {
            try
            {
                TrainingModel trainingModel = GetAll()
                    .Where(x => x.TrainingId != training.TrainingId)
                    .FirstOrDefault(x => x.TrainingName.Equals(training.TrainingName));
                CheckInsertUpdateDuplicate(trainingModel);
                await this._trainingDAL.UpdateAsync(training);
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

        public IEnumerable<TrainingModel> GetNotEnrolledTrainings(int userId)
        {
            return this._trainingDAL.GetNotEnrolledTrainings(userId);
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
