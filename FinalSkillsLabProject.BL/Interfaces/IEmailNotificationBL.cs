using FinalSkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.BL.Interfaces
{
    public interface IEmailNotificationBL
    {
        void SendApprovalRejectionEmail(bool isApproved, string recipient, string username, string training, string requestHandlerName, string requestHandlerRole, string requestHandlerEmail, string declineReason, string managerEmail);
        void SendSelectionRejectionEmail(bool isSelected, EnrollmentSelectionViewModel enrollment);
    }
}
