using System;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Exceptions;
using FinalSkillsLabProject.DAL.Interfaces;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL
{
    public class AccountBL : IAccountBL
    {
        // DIP
        private readonly IAccountDAL _accountDAL;

        public AccountBL(IAccountDAL accountDAL)
        {
            this._accountDAL = accountDAL;
        }

        public bool AuthenticateUser(LoginModel model)
        {
            return this._accountDAL.AuthenticateUser(model);
        }
        public LoginModel GetByUsername(string username)
        {
            return this._accountDAL.GetByUsername(username);
        }

        public string Update(AccountModel account)
        {
            try
            {
                CheckUpdateDuplicate(account.UserId, account.Username);
                this._accountDAL.Update(account);
                return "Account successfully updated!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void CheckUpdateDuplicate(int userId, string username)
        {
            AccountModel account = this._accountDAL.GetByUsernameAndUserId(username, userId);
            string message = "";
            if (account != null)
            {
                message = "Username already exists!";
                throw new DuplicationException(message);
            }
        }
    }
}
