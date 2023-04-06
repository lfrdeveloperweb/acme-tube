using AcmeTube.Application.Services;
using Autofac;

namespace AcmeTube.IoC.Modules
{
    public sealed class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            

            // builder.Register(_ => _configuration.GetSection("orderAttachmentSettings").Get<OrderAttachmentSettings>());
        }
    }
}
