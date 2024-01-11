using FinalSkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IExportBL
    {
        bool ExportToExcel(List<UserViewModel> selectedList, string trainingName);
    }
}
