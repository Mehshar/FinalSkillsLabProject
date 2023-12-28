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
                TrainingModel trainingModel = (await GetAllAsync()).FirstOrDefault(x => x.TrainingName.Equals(training.TrainingName));
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
                TrainingModel trainingModel = (await GetAllAsync())
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

        public async Task<bool> DeleteAsync(int trainingId)
        {
            return await this._trainingDAL.DeleteAsync(trainingId);
        }

        public async Task<TrainingModel> GetAsync(int trainingId)
        {
            return await this._trainingDAL.GetAsync(trainingId);
        }

        public async Task<IEnumerable<TrainingModel>> GetAllAsync()
        {
            return await this._trainingDAL.GetAllAsync();
        }

        public async Task<IEnumerable<TrainingModel>> GetAllByUserAsync(int userId)
        {
            return await this._trainingDAL.GetAllByUserAsync(userId);
        }

        public async Task<IEnumerable<TrainingModel>> GetNotEnrolledTrainingsAsync(int userId)
        {
            return await this._trainingDAL.GetNotEnrolledTrainingsAsync(userId);
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
