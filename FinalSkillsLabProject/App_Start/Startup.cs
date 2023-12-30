using FinalSkillsLabProject.Authorization;
using FinalSkillsLabProject.BL.Interfaces;
using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(FinalSkillsLabProject.App_Start.Startup))]

namespace FinalSkillsLabProject.App_Start
{
    public class Startup
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["HangFireConnectionString"].ConnectionString;
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(_connectionString)
                .UseActivator(new ContainerJobActivator(UnityConfig.UnityContainer));

            yield return new BackgroundJobServer();
        }

        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });

            BackgroundJob.Schedule<IEnrollmentBL>(x => x.EnrollmentAutomaticProcessing(), TimeSpan.FromMinutes(1));
        }
    }
}
