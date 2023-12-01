using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IAccountBL
    {
        bool AuthenticateUser(LoginModel model);
        LoginModel GetByUsername(string username);
    }
}
