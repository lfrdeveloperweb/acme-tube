using AcmeTube.Application.DependencyInjection;
using AcmeTube.Infrastructure.DependencyInjection;
using AcmeTube.IoC.Modules;
using Autofac;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

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
                // Exclude all types that extends from INotificationHandler to avoid duplicate registers with the extension 'AddMediatR' present in 'Program'.
				.Where(type => !type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
				//.Where(type => !type.IsAssignableFrom(typeof(INotificationHandler<>)))
                //.Where(type => !(type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            builder.RegisterModule(new DataModule(configuration));
            builder.RegisterModule<CoreModule>();
            //builder.RegisterModule<ApplicationModule>();
        }
	}
}
