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
    public class PrerequisiteBL : IPrerequisiteBL
    {
        private readonly IPrerequisiteDAL _prerequisiteDAL;

        public PrerequisiteBL(IPrerequisiteDAL prerequisiteDAL)
        {
            this._prerequisiteDAL = prerequisiteDAL;
        }

        public async Task<string> AddAsync(PrerequisiteModel prerequisite, int trainingId)
        {
            try
            {
                PrerequisiteModel prerequisiteModel = (await GetAllAsync()).FirstOrDefault(x => x.Description.Equals(prerequisite.Description));
                CheckInsertUpdateDuplicate(prerequisiteModel);
                await this._prerequisiteDAL.AddAsync(prerequisite, trainingId);
                return "Prerequisite added successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdateAsync(PrerequisiteModel prerequisite)
        {
            try
            {
                PrerequisiteModel prerequisiteModel = (await GetAllAsync())
                    .Where(x => x.PrerequisiteId != prerequisite.PrerequisiteId)
                    .FirstOrDefault(x => x.Description.Equals(prerequisite.Description));
                CheckInsertUpdateDuplicate(prerequisiteModel);
                await this._prerequisiteDAL.UpdateAsync(prerequisite);
                return "Prerequisite updated successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<bool> DeleteAsync(int prerequisiteId)
        {
            return await this._prerequisiteDAL.DeleteAsync(prerequisiteId);
        }

        public async Task<IEnumerable<PrerequisiteModel>> GetAllAsync()
        {
            return await this._prerequisiteDAL.GetAllAsync();
        }

        public async Task<IEnumerable<PrerequisiteModel>> GetAllByTrainingAsync(int trainingId)
        {
            return await this._prerequisiteDAL.GetAllByTrainingAsync(trainingId);
        }

        private void CheckInsertUpdateDuplicate(PrerequisiteModel prerequisite)
        {
            if (prerequisite != null)
            {
                string message = "Prerequisite already exists!";
                throw new DuplicationException(message);
            }
        }
    }
}
