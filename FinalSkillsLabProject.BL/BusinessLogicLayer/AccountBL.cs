using System;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Exceptions;
using FinalSkillsLabProject.DAL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using System.Threading.Tasks;
using FinalSkillsLabProject.BL.BusinessLogicLayer;
using System.Linq;

namespace FinalSkillsLabProject.BL
{
    public class AccountBL : IAccountBL
    {
        private readonly IAccountDAL _accountDAL;

        public AccountBL(IAccountDAL accountDAL)
        {
            this._accountDAL = accountDAL;
        }

        public async Task<bool> AuthenticateUserAsync(LoginModel model)
        {
            (byte[] storedHashedPassword, byte[] salt) = await _accountDAL.AuthenticateUserAsync(model);

            if (storedHashedPassword != null && salt != null)
            {
                (byte[] hashedPassword, _) = HashingBL.HashPassword(model.Password, salt);
                return hashedPassword.SequenceEqual(storedHashedPassword);
            }
            return false;
        }
        public async Task<UserViewModel> GetByUsernameAsync(string username)
        {
            return await this._accountDAL.GetByUsernameAsync(username);
        }

        public async Task<string> UpdateAsync(AccountModel account)
        {
            try
            {
                await CheckUpdateDuplicate(account.UserId, account.Username);
                await this._accountDAL.UpdateAsync(account);
                return "Account successfully updated!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task CheckUpdateDuplicate(int userId, string username)
        {
            AccountModel account = await this._accountDAL.GetByUsernameAndUserIdAsync(username, userId);
            string message = "";
            if (account != null)
            {
                message = "Username already exists!";
                throw new DuplicationException(message);
            }
        }
    }
}
