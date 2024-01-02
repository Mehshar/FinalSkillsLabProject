using FinalSkillsLabProject.Common.ErrorLogging;
using FinalSkillsLabProject.ExceptionHandling;
using System.Web.Mvc;
using Unity;

namespace FinalSkillsLabProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            ILogger logger = UnityConfig.UnityContainer.Resolve<ILogger>(); // Resolve the logger using Unity

            // The order of filters matters, and for the global exception filter must execute before other filters
            filters.Add(new GlobalExceptionFilter(logger));
            filters.Add(new HandleErrorAttribute());
        }
    }
}
