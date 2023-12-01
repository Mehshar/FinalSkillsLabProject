using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.DAL.Interfaces
{
    public interface IPendingUserDAL
    {
        bool Add(PendingUserModel model);
        PendingUserModel GetById(int id);
        IEnumerable<PendingUserModel> GetAll();

    }
}
