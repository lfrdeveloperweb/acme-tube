﻿using System.Reflection;
using AcmeTube.IoC.Modules;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeTube.IoC
{
    public static class InjectorBootstrapper
    {
        /// <summary>
        /// Makes use of Autofac to set up the service provider, bringing together Autofac registrations and ASP.NET Core framework registrations.
        /// </summary>
        public static void Inject(ContainerBuilder builder, IConfiguration configuration, IServiceCollection services, Assembly apiAssembly)
        {
            var assemblies = new[]
            {
                apiAssembly,
                Application.AssemblyReference.Assembly, 
                Data.AssemblyReference.Assembly,
                Infrastructure.AssemblyReference.Assembly
            };

            builder
                .RegisterAssemblyTypes(assemblies)
                // Exclude all types that extends from INotificationHandler by avoid duplicate registers with the extension 'AddMediatR' present in 'Startup'.
                //.Where(type => !type.IsAssignableToGenericType(typeof(INotificationHandler<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterModule(new DataModule(configuration));
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<ApplicationModule>();
        }
    }
}
