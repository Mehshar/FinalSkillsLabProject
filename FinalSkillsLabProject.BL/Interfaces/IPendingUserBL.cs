using FinalSkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IPendingUserBL
    {
        string Add(PendingUserModel pendingUser);
        IEnumerable<PendingUserModel> GetAll();

    }
}
