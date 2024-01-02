using FinalSkillsLabProject.Common.ErrorLogging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
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
            //if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            //{
            //    Debug.WriteLine("Exception might have been handled");
            //    return;
            //}

            LogException(filterContext.Exception);

            // Add debug statement
            Debug.WriteLine("Exception caught by GlobalExceptionFilter");

            // Redirect to error view
            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "Error",
            //};

            filterContext.ExceptionHandled = true;
        }

        private void LogException(Exception exception)
        {
            // Log the exception using the custom logger
            //CustomException customException = new CustomException("Global Exception Filter", exception, _logger);
            //customException.Log();

            _logger.LogError(exception);
        }
    }
}