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
using Hangfire;
using System.Net.Http;

namespace FinalSkillsLabProject.BL.BusinessLogicLayer
{
    public class EmailNotificationBL : IEmailNotificationBL
    {
        private readonly string sender;
        private readonly string password;
        private string smtpServer;
        private readonly int port;

        public EmailNotificationBL()
        {
            smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            port = int.Parse(ConfigurationManager.AppSettings["port"]);
            sender = ConfigurationManager.AppSettings["sender"];
            password = ConfigurationManager.AppSettings["password"];
        }

        public void SendApprovalRejectionEmail(bool isApproved, string recipient, string username, string training, string requestHandlerName, string requestHandlerRole, string requestHandlerEmail, string declineReason, string managerEmail)
        {
            string subject = GetApprovalRejectionSubject(isApproved);
            string body = GetApprovalRejectionEmailBody(username, training, isApproved, requestHandlerName, requestHandlerRole, declineReason);
            MailMessage mailMessage = CreateMailMessage(sender, recipient, subject, body);
            mailMessage.CC.Add(requestHandlerEmail);
            if (requestHandlerRole.Equals(RoleEnum.Admin.ToString().ToLower())) { mailMessage.CC.Add(managerEmail); }

            SendEmail(mailMessage);
        }

        public void SendSelectionRejectionEmail(bool isSelected, EnrollmentSelectionViewModel enrollment)
        {
            string subject = GetSelectionEmailSubject(isSelected);
            string body = GetSelectionEmailBody(isSelected, enrollment);
            MailMessage mailMessage = CreateMailMessage(sender, enrollment.EmployeeEmail, subject, body);
            mailMessage.CC.Add(enrollment.ManagerEmail);

            SendMailInBackground(mailMessage);
        }

        private MailMessage CreateMailMessage(string sender, string recipient, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage(sender, recipient)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            return mailMessage;
        }

        private string GetApprovalRejectionSubject(bool isApproved)
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

        private string GetApprovalRejectionEmailBody(string username, string training, bool isApproved, string requestHandlerName, string requestHandlerRole, string declineReason)
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
                             {requestHandlerRole}, <strong>{requestHandlerName}</strong>.</p>
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
                             {requestHandlerRole}, <strong>{requestHandlerName}</strong>.</p>
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

        private void SendEmail(MailMessage mailMessage)
        {
            using (SmtpClient smtpClient = new SmtpClient(smtpServer)
            {
                Port = port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, password)
            })
            {
                try
                {
                    Task.Run(() => smtpClient.SendMailAsync(mailMessage)).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void SendMailInBackground(MailMessage mailMessage)
        {
            using (SmtpClient smtpClient = new SmtpClient(smtpServer)
            {
                Port = port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, password)
            })
            {
                smtpClient.Send(mailMessage);
            }
        }
    }
}
