using FinalSkillsLabProject.Common.ErrorLogging;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace FinalSkillsLabProject.ExceptionHandling
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            LogException(filterContext.Exception);

            Debug.WriteLine("Exception caught by GlobalExceptionFilter");

            filterContext.ExceptionHandled = true;
        }

        private void LogException(Exception exception)
        {
            _logger.LogError(exception);
        }
    }
}