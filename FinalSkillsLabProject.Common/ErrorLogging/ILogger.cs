using System;

namespace FinalSkillsLabProject.Common.ErrorLogging
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(Exception exception);
    }
}