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
    public class PrerequisiteBL : IPrerequisiteBL
    {
        private readonly IPrerequisiteDAL _prerequisiteDAL;

        public PrerequisiteBL(IPrerequisiteDAL prerequisiteDAL)
        {
            this._prerequisiteDAL = prerequisiteDAL;
        }

        public string Add(PrerequisiteModel prerequisite, int trainingId)
        {
            try
            {
                PrerequisiteModel prerequisiteModel = GetAll().FirstOrDefault(x => x.Description.Equals(prerequisite.Description));
                CheckInsertUpdateDuplicate(prerequisiteModel);
                this._prerequisiteDAL.Add(prerequisite, trainingId);
                return "Prerequisite added successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string Update(PrerequisiteModel prerequisite)
        {
            try
            {
                PrerequisiteModel prerequisiteModel = GetAll()
                    .Where(x => x.PrerequisiteId != prerequisite.PrerequisiteId)
                    .FirstOrDefault(x => x.Description.Equals(prerequisite.Description));
                CheckInsertUpdateDuplicate(prerequisiteModel);
                this._prerequisiteDAL.Update(prerequisite);
                return "Prerequisite updated successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool Delete(int prerequisiteId)
        {
            return this._prerequisiteDAL.Delete(prerequisiteId);
        }

        public IEnumerable<PrerequisiteModel> GetAll()
        {
            return this._prerequisiteDAL.GetAll();
        }

        public IEnumerable<PrerequisiteModel> GetAllByTraining(int trainingId)
        {
            return this._prerequisiteDAL.GetAllByTraining(trainingId);
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
