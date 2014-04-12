[assembly: WebActivator.PreApplicationStartMethod(typeof(Banking.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Banking.App_Start.NinjectWebCommon), "Stop")]

namespace Banking.App_Start
{
    using System;
    using System.Web;

    using Banking.Application.DAL;
    using Banking.Domain.Services.BankingOperationsEngine;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            kernel.Bind<IAccountRepository>().To<AccountRepository>();
            kernel.Bind<ITransactionRepository>().To<TransactionRepository>();
            kernel.Bind<ICustomerOperationsManager>().To<CustomerOperationsManager>();
            kernel.Bind<IAccountOperationsManager>().To<AccountOperationsManager>();
            kernel.Bind<ITransactionEngine>().To<TransactionEngine>();
            kernel.Bind<ITimeProvider>().To<TimeProvider>();
            kernel.Bind<IInvestmentManager>().To<InvestmentManager>();
            kernel.Bind<IInvestmentRepository>().To<InvestmentRepository>();
        }        
    }
}
