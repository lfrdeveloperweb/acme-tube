using AcmeTube.Application.Services;
using Autofac;

namespace AcmeTube.IoC.Modules
{
    public sealed class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountAppService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ChannelAppService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<VideoAppService>()
                .InstancePerLifetimeScope();

            // builder.Register(_ => _configuration.GetSection("orderAttachmentSettings").Get<OrderAttachmentSettings>());
        }
    }
}
