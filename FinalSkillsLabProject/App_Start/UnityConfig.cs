using FinalSkillsLabProject.BL.BusinessLogicLayer;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.BL;
using FinalSkillsLabProject.DAL.DataAccessLayer;
using FinalSkillsLabProject.DAL.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace FinalSkillsLabProject
{
    public static class UnityConfig
    {
        public static IUnityContainer UnityContainer { get; internal set; }

        public static void RegisterComponents()
        {
            UnityContainer = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            UnityContainer.RegisterType<IAccountDAL, AccountDAL>();
            UnityContainer.RegisterType<IAccountBL, AccountBL>();

            UnityContainer.RegisterType<IDepartmentBL, DepartmentBL>();
            UnityContainer.RegisterType<IDepartmentDAL, DepartmentDAL>();

            UnityContainer.RegisterType<IEnrollmentBL, EnrollmentBL>();
            UnityContainer.RegisterType<IEnrollmentDAL, EnrollmentDAL>();

            UnityContainer.RegisterType<IPrerequisiteBL, PrerequisiteBL>();
            UnityContainer.RegisterType<IPrerequisiteDAL, PrerequisiteDAL>();

            UnityContainer.RegisterType<ITrainingBL, TrainingBL>();
            UnityContainer.RegisterType<ITrainingDAL, TrainingDAL>();

            UnityContainer.RegisterType<IUserBL, UserBL>();
            UnityContainer.RegisterType<IUserDAL, UserDAL>();

            UnityContainer.RegisterType<IEmailNotificationBL, EmailNotificationBL>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityContainer));
        }
    }
}