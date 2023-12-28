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
    public class UserBL : IUserBL
    {
        private readonly IUserDAL _userDAL;
        private readonly IAccountDAL _accountDAL;

        public UserBL(IUserDAL userDAL, IAccountDAL accountDAL)
        {
            this._userDAL = userDAL;
            this._accountDAL = accountDAL;
        }

        public async Task<string> AddAsync(SignUpModel model)
        {
            try
            {
                await CheckInsertDuplicate(model.NIC, model.Email, model.MobileNum, model.Username);
                await this._userDAL.AddAsync(model);
                return "Account created successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateAsync(UserModel user)
        {
            try
            {
                await CheckUpdateDuplicate(user.UserId, user.NIC, user.Email, user.MobileNum);
                await this._userDAL.UpdateAsync(user);
                return "User updated successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            return await this._userDAL.DeleteAsync(userId);
        }

        public async Task<UserModel> GetAsync(int userId)
        {
            return await this._userDAL.GetAsync(userId);
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return await this._userDAL.GetAllAsync();
        }

        // Validations
        private async Task CheckInsertDuplicate(string nic, string email, string mobileNum, string username)
        {
            List<UserModel> usersList = (await GetAllAsync()).ToList();
            UserViewModel account = await this._accountDAL.GetByUsernameAsync(username);
            string message = "";

            // true when there is NIC duplication
            if (usersList.FirstOrDefault(x => x.NIC.Equals(nic)) != null)
            {
                message = "NIC already exists!";
            }

            // true when there is email duplication
            if (usersList.FirstOrDefault(x => x.Email.Equals(email)) != null)
            {
                message = "Email already exists!";
            }

            // true when there is mobile number duplication
            if (usersList.FirstOrDefault(x => x.MobileNum.Equals(mobileNum)) != null)
            {
                message = "Mobile number already exists!";
            }

            // true when there is username duplication
            if (account != null)
            {
                message = "Username already exists!";
            }

            if (!string.IsNullOrEmpty(message))
            {
                throw new DuplicationException(message);
            }
        }

        private async Task CheckUpdateDuplicate(int userId, string nic, string email, string mobileNum)
        {
            List<UserModel> usersList = (await GetAllAsync()).Where(x => x.UserId != userId).ToList();
            string message = "";

            if (usersList.FirstOrDefault(x => x.NIC.Equals(nic)) != null)
            {
                message = "NIC already exists!";
            }

            if (usersList.FirstOrDefault(x => x.Email.Equals(email)) != null)
            {
                message = "Email already exists!";
            }

            if (usersList.FirstOrDefault(x => x.MobileNum.Equals(mobileNum)) != null)
            {
                message = "Mobile number already exists";
            }

            if (!string.IsNullOrEmpty(message))
            {
                throw new DuplicationException(message);
            }
        }
    }
}
