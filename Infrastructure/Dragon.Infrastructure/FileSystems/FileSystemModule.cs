using Autofac;

namespace Auth.Infrastructure.FileSystems
{
    public class FileSystemModule : Module
    {
        #region Overrides of Module

        /// <summary>Override to add registrations to the container.</summary>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<UploadFolder>().As<IUploadFolder>().InstancePerMatchingLifetimeScope("shell");
            //builder.RegisterType<UploadFileFolder>().As<IUploadFileFolder>().InstancePerMatchingLifetimeScope("shell");
            //builder.RegisterType<UploadImageFolder>().As<IUploadImageFolder>().InstancePerMatchingLifetimeScope("shell");
            builder.RegisterType<UploadFolder>().As<IUploadFolder>();
            builder.RegisterType<UploadFileFolder>().As<IUploadFileFolder>();
            builder.RegisterType<UploadImageFolder>().As<IUploadImageFolder>();
        }

        #endregion Overrides of Module
    }
}
