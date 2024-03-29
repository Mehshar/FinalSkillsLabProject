﻿using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Exceptions;
using FinalSkillsLabProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Enums;

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

        public async Task<string> UpdateAsync(TrainingPrerequisiteViewModel training)
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

        public async Task<IEnumerable<TrainingModel>> GetByDeadlineAsync()
        {
            return await this._trainingDAL.GetByDeadlineAsync();
        }

        public async Task<int> GetNotEnrolledTrainingsCountAsync(int userId)
        {
            return await this._trainingDAL.GetNotEnrolledTrainingsCountAsync(userId);
        }

        public async Task<IEnumerable<TrainingModel>> GetNotEnrolledTrainingsPagedAsync(int userId, int page, int pageSize)
        {
            return await this._trainingDAL.GetNotEnrolledTrainingsPagedAsync(userId, page, pageSize);
        }

        public async Task<TrainingPrerequisiteViewModel> GetWithPrerequisitesAsync(int trainingId)
        {
            return await this._trainingDAL.GetWithPrerequisitesAsync(trainingId);
        }

        public async Task<bool> IsEnrollmentAsync(int trainingId)
        {
            return await this._trainingDAL.IsEnrollmentAsync(trainingId);
        }

        public async Task<IEnumerable<UserViewModel>> GetByStatus(int trainingId, EnrollmentStatusEnum status)
        {
            return await this._trainingDAL.GetByStatus(trainingId, status);
        }

        public TrainingValidationResult CheckTraining(TrainingPrerequisiteViewModel training)
        {
            if (training.IsDeleted) 
            {
                return new TrainingValidationResult { IsValid = false, Message = "Training already deleted" };
            }

            if (training.Deadline < DateTime.Now) 
            {
                return new TrainingValidationResult { IsValid = false, Message = "Training deadline has passed" };
            }
            return new TrainingValidationResult { IsValid = true, Message = "" };
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
