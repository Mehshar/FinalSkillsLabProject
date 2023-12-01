using FinalSkillsLabProject.Common.Exceptions;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.BL.Interfaces;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class PendingUserBL : IPendingUserBL
    {
        private readonly IPendingUserDAL _pendingUserDAL;
        private readonly IUserDAL _userDAL;
        private readonly IAccountDAL _accountDAL;

        public PendingUserBL(IPendingUserDAL pendingUserDAL, IUserDAL userDAL, IAccountDAL accountDAL)
        {
            _pendingUserDAL = pendingUserDAL;
            _userDAL = userDAL;
            _accountDAL = accountDAL;
        }

        public string Add(PendingUserModel pendingUser)
        {
            try
            {
                CheckInsertDuplicate(pendingUser.NIC, pendingUser.Email, pendingUser.MobileNum, pendingUser.Username);
                _pendingUserDAL.Add(pendingUser);
                return "Registration submitted successfully!";
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IEnumerable<PendingUserModel> GetAll()
        {
            return _pendingUserDAL.GetAll();
        }

        private void CheckInsertDuplicate(string nic, string email, string mobileNum, string username)
        {
            List<PendingUserModel> pendingUsersList = GetAll().ToList();        // checking for NIC, Email and MobileNum duplication amongst pending users
            List<UserModel> usersList = _userDAL.GetAll().ToList();             // checking for NIC, Email and MobileNum duplication amongst approved users
            LoginModel loginModel = _accountDAL.GetByUsername(username);        // checking for username duplication amongst approved and pending users
            string message = "";

            if (pendingUsersList.FirstOrDefault(x => x.NIC.Equals(nic)) != null || usersList.FirstOrDefault(x => x.NIC.Equals(nic)) != null)
            {
                message = "NIC already exists";
            }

            if (pendingUsersList.FirstOrDefault(x => x.Email.Equals(email)) != null || usersList.FirstOrDefault(x => x.Email.Equals(email)) != null)
            {
                message = "Email already exists";
            }

            if (pendingUsersList.FirstOrDefault(x => x.MobileNum.Equals(mobileNum)) != null || usersList.FirstOrDefault(x => x.MobileNum.Equals(mobileNum)) != null)
            {
                message = "Mobile number already exists";
            }

            if (loginModel != null || pendingUsersList.FirstOrDefault(x => x.Username.Equals(username)) != null)
            {
                message = "Username already exists";
            }

            if (!string.IsNullOrEmpty(message))
            {
                throw new DuplicationException(message);
            }
        }
    }
}
