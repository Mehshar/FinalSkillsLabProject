using FinalSkillsLabProject.Common.Models;
using System.Collections.Generic;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IExportBL
    {
        bool ExportToExcel(List<UserViewModel> selectedList, string trainingName);
    }
}
