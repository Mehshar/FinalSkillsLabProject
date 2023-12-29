using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

namespace FinalSkillsLabProject.App_Start
{
    public class ContainerJobActivator : JobActivator
    {
        private IUnityContainer _container;

        public ContainerJobActivator(IUnityContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type jobType)
        {
            return _container.Resolve(jobType);
        }
    }
}