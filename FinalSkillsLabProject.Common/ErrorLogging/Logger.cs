using Microsoft.Owin.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.Common.ErrorLogging
{
    public class Logger : ILogger
    {
        private readonly string logFilePath;

        public Logger(string filePath = "Logs\\log.txt")
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            logFilePath = Path.Combine(baseDirectory, filePath);

            // Use Server.MapPath to get the physical path in an ASP.NET application
            //string baseDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~");
            //logFilePath = Path.Combine(baseDirectory, filePath);
        }

        public void Log(string message)
        {
            string logMessage = $"{message}";
            Debug.WriteLine(logFilePath);

            try
            {
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath).Close(); // Close the file stream after creation
                }

                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(logMessage);
                    Debug.WriteLine("Error message logged in log.txt");
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        public void LogError(Exception exception)
        {
            string fullMessage = Environment.NewLine + "--------------------------------------------------";
            fullMessage += Environment.NewLine + $"Timestamp: {DateTime.Now}";
            fullMessage += Environment.NewLine + $"Exception Type: {exception.GetType().FullName}";
            fullMessage += Environment.NewLine + $"Message: {exception.Message}";
            fullMessage += Environment.NewLine + $"Inner Exception: {exception.InnerException}";
            fullMessage += Environment.NewLine + $"Stack Trace: {exception.StackTrace}";
            fullMessage += Environment.NewLine + "--------------------------------------------------";

            Log(fullMessage);
        }
    }
}
