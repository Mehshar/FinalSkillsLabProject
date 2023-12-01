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
    public class UserBL : IUserBL
    {
        private readonly IUserDAL _userDAL;
        private readonly IAccountDAL _accountDAL;

        public UserBL(IUserDAL userDAL, IAccountDAL accountDAL)
        {
            this._userDAL = userDAL;
            this._accountDAL = accountDAL;
        }

        public string Add(SignUpModel model)
        {
            try
            {
                CheckInsertDuplicate(model.NIC, model.Email, model.MobileNum, model.Username);
                this._userDAL.Add(model);
                return "Account created successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Update(UserModel user)
        {
            try
            {
                CheckUpdateDuplicate(user.UserId, user.NIC, user.Email, user.MobileNum);
                this._userDAL.Update(user);
                return "User updated successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool Delete(int userId)
        {
            return this._userDAL.Delete(userId);
        }

        public UserModel Get(int userId)
        {
            return this._userDAL.Get(userId);
        }

        public IEnumerable<UserModel> GetAll()
        {
            return this._userDAL.GetAll();
        }

        // Validations
        private void CheckInsertDuplicate(string nic, string email, string mobileNum, string username)
        {
            List<UserModel> usersList = GetAll().ToList();
            LoginModel account = this._accountDAL.GetByUsername(username);
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

        private void CheckUpdateDuplicate(int userId, string nic, string email, string mobileNum)
        {
            List<UserModel> usersList = GetAll().Where(x => x.UserId != userId).ToList();
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
