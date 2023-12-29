using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Enums;
using FinalSkillsLabProject.Common.Models;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class EmailNotificationBL : IEmailNotificationBL
    {
        public void SendEmail(bool isApproved, string recipient, string username, string training, string requestHandlerName, string requestHandlerRole, string requestHandlerEmail, string declineReason, string managerEmail)
        {
            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            string sender = ConfigurationManager.AppSettings["sender"];
            string password = ConfigurationManager.AppSettings["password"];

            var smtpClient = new SmtpClient(smtpServer);

            smtpClient.Port = port;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(sender, password);

            var mailMessage = new MailMessage(sender, recipient);

            mailMessage.CC.Add(requestHandlerEmail);
            if (requestHandlerRole.Equals(RoleEnum.Admin.ToString().ToLower())) { mailMessage.CC.Add(managerEmail); }
            mailMessage.Subject = GetEmailSubject(isApproved);
            mailMessage.Body = GetEmailBody(username, training, isApproved, requestHandlerName, requestHandlerRole, declineReason);
            mailMessage.IsBodyHtml = true;

            try
            {
                Task.Run(() => smtpClient.SendMailAsync(mailMessage)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendSelectionEmail(bool isSelected, EnrollmentSelectionViewModel selectedEnrollment)
        {
            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            string sender = ConfigurationManager.AppSettings["sender"];
            string password = ConfigurationManager.AppSettings["password"];

            var smtpClient = new SmtpClient(smtpServer);

            smtpClient.Port = port;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(sender, password);

            var mailMessage = new MailMessage(sender, selectedEnrollment.EmployeeEmail);

            mailMessage.CC.Add(selectedEnrollment.ManagerEmail);
            mailMessage.Subject = GetSelectionEmailSubject(isSelected);
            mailMessage.Body = GetSelectionEmailBody(isSelected, selectedEnrollment);
            mailMessage.IsBodyHtml = true;

            try
            {
                Task.Run(() => smtpClient.SendMailAsync(mailMessage)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetEmailSubject(bool isApproved)
        {
            string result = isApproved ? "Approved" : "Rejected";
            string subject = $"Training Request - {result}";
            return subject;
        }

        private string GetSelectionEmailSubject(bool isSelected)
        {
            string result = isSelected ? "Selected" : "Rejected";
            string subject = $"Training Request - {result}";
            return subject;
        }

        private string GetEmailBody(string username, string training, bool isApproved, string requestHandler, string requestHandlerRole, string declineReason)
        {
            string result = isApproved ? "approved" : "rejected";
            string htmlBody;

            if (isApproved)
            {
                htmlBody =
                    $@"
                    <html>
                        <head>
                            <title>Enrollment Response</title>
                        </head>

                        <body>
                            <p>Hello <strong>{username}</strong>,</p>
                            <p>Your enrollment request for the training, <strong>{training}</strong>, has been <strong>{result}</strong> by the
                             {requestHandlerRole}, <strong>{requestHandler}</strong>.</p>
                            <p>Please liaise with your manager for further information.</p>
                            <p>Regards, </br>SkillsHub Team</p>
                        </body>
                    </html>";
            }

            else
            {
                htmlBody =
                    $@"
                    <html>
                        <head>
                            <title>Enrollment Response</title>
                        </head>

                        <body>
                            <p>Hello <strong>{username}</strong>,</p>
                            <p>Your enrollment request for the training, <strong>{training}</strong>, has been <strong>{result}</strong> by the
                             {requestHandlerRole}, <strong>{requestHandler}</strong>.</p>
                            <p><strong>Reason for Decline: </strong>{declineReason}</p>
                            <p>Please liaise with your manager for further information.</p>
                            <p>Regards, </br>SkillsHub Team</p>
                        </body>
                    </html>";
            }
            return htmlBody;
        }

        private string GetSelectionEmailBody(bool isSelected, EnrollmentSelectionViewModel selectedEnrollment)
        {
            string result = isSelected ? "selected" : "rejected";
            string htmlBody;

            if (isSelected)
            {
                htmlBody =
                    $@"
                    <html>
                        <head>
                            <title>Enrollment Response</title>
                        </head>

                        <body>
                            <p>Hello <strong>{selectedEnrollment.EmployeeUsername}</strong>,</p>
                            <p>You have been selected for the training, <strong>{selectedEnrollment.TrainingName}</strong>, that closed on {selectedEnrollment.Deadline.ToShortDateString()}.</p>
                            <p>Please liaise with your manager, <strong>{selectedEnrollment.ManagerFirstName} {selectedEnrollment.ManagerLastName}</strong>, for further information.</p>
                            <p>Regards, </br>SkillsHub Team</p>
                        </body>
                    </html>";
            }

            else
            {
                htmlBody =
                    $@"
                    <html>
                        <head>
                            <title>Enrollment Response</title>
                        </head>

                        <body>
                            <p>Hello <strong>{selectedEnrollment.EmployeeUsername}</strong>,</p>
                            <p>Your enrollment request for the training, <strong>{selectedEnrollment.TrainingName}</strong>, has been <strong>{result}</strong>.</p>
                            <p><strong>Reason for Decline: </strong>The training capacity has been reached and you did not have priority.</p>
                            <p>Regards, </br>SkillsHub Team</p>
                        </body>
                    </html>";
            }
            return htmlBody;
        }
    }
}
