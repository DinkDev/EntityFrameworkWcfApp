﻿namespace EntityFrameworkWpfApp
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Autofac;
    using Caliburn.Micro;
    using DataAccess;
    using Domain;
    using ViewModels;

    /// <remarks>
    /// Autofac wire up borrowed from: <see href="http://grantbyrne.com/post/settingupautofacwithcaliburnmicro/">Grant's Blog</see>
    /// </remarks>>
    public class AppBootstrapper : BootstrapperBase
    {
        private IContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            // register types generally
            var local = Assembly.GetExecutingAssembly();
            var caliburn = Assembly.GetAssembly(typeof(WindowManager));

            builder.RegisterAssemblyTypes(local, caliburn)
                .AsSelf()
                .AsImplementedInterfaces();

            // register modules
            builder.RegisterModule(new DomainModule());
            builder.RegisterModule(new DataAccessModule());

            //builder.RegisterModule(new DataAccessModule { DbOptions = dbOptions });
            //builder.RegisterType<Settings>().As<IPackageManagerSettings>().SingleInstance();

            // register specific types
            builder.RegisterType<ShellViewModel>().As<IShell>().SingleInstance();
            //builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
            //builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
            //builder.RegisterType<Settings>().SingleInstance();

            //var dbOptions = new DbContextOptionsBuilder()
            //    .UseSqlServer(configuration[@"ConnectionStrings:CdcConnection"])
            //    .Options;

            //builder.RegisterInstance(dbOptions).SingleInstance();

            _container = builder.Build();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                if (_container.IsRegistered(service))
                {
                    return _container.Resolve(service);
                }
            }
            else if (_container.IsRegisteredWithKey(key, service))
            {
                return _container.ResolveKeyed(key, service);
            }

            var keyMessage = string.IsNullOrWhiteSpace(key) ? string.Empty : $"Key: {key}, ";
            var msg = $"Unable to find registration for {keyMessage}Service: {service.Name}.";
            throw new Exception(msg);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(service);
            return _container.Resolve(type) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            //var loggerFactory = _container.Resolve<ILoggerFactory>();
            //loggerFactory
            //    .AddSerilog();

            DisplayRootViewFor<IShell>();
        }
    }
}
