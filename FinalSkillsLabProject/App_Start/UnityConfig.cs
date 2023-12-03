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
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IAccountDAL, AccountDAL>();
            container.RegisterType<IAccountBL, AccountBL>();

            container.RegisterType<IDepartmentBL, DepartmentBL>();
            container.RegisterType<IDepartmentDAL, DepartmentDAL>();

            container.RegisterType<IEnrollmentBL, EnrollmentBL>();
            container.RegisterType<IEnrollmentDAL, EnrollmentDAL>();

            container.RegisterType<IPrerequisiteBL, PrerequisiteBL>();
            container.RegisterType<IPrerequisiteDAL, PrerequisiteDAL>();

            container.RegisterType<ITrainingBL, TrainingBL>();
            container.RegisterType<ITrainingDAL, TrainingDAL>();

            container.RegisterType<IUserBL, UserBL>();
            container.RegisterType<IUserDAL, UserDAL>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}