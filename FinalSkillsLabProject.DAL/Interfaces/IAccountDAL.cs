using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IAccountDAL
    {
        bool AuthenticateUser(LoginModel model);
        LoginModel GetByUsername(string username);
        AccountModel GetByUsernameAndUserId(string username, int userId);
        bool Update(AccountModel account);
    }
}
