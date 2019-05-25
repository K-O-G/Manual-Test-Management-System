using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using Domain.Abstract;
using Domain.Abstract.CheckListAbstract;
using Domain.Abstract.TestCaseAbstract;
using Domain.Concrete;
using Domain.Concrete.CheckList;
using Domain.Concrete.TestCase;
using Domain.Entities;
using Domain.Entities.CheckLists;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IComponentRepository>().To<EFComponentRepository>();
            kernel.Bind<ICheckListRepository>().To<EFCheckListRepository>();
            kernel.Bind<ICheckListItemRepository>().To<EFCheckListItemRepository>();
            kernel.Bind<ICaseRepository>().To<EFCaseRepository>();
            kernel.Bind<ICaseStepRepository>().To<EFCaseStepRepository>();
            kernel.Bind<ITestCaseRepository>().To<EFTestCaseRepository>();
            kernel.Bind<IPriorityRepository>().To<EFPriorityRepository>();
            kernel.Bind<ITestResultRepository>().To<EFTestResultRepository>();
            kernel.Bind<IUserRepository>().To<EFUserRepository>();
        }
    }
}
