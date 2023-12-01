using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IUserBL
    {
        string Add(SignUpModel model);
        string Update(UserModel user);
        bool Delete(int userId);
        UserModel Get(int userId);
        IEnumerable<UserModel> GetAll();
    }
}
