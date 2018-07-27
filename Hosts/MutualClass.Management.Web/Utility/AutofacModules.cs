using Autofac;
using Sample.Management.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Utility
{
    public class AutofacModules : Module
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
            builder.RegisterType<AdminMenu>().AsSelf();
            builder.RegisterType<UserService>().AsSelf();
            builder.RegisterType<WebApiAgentService>().AsSelf();
            builder.RegisterType<PublishSampleQuesRecycService>().AsSelf(); 
            builder.RegisterType<PublishSampleCouponRecycService>().AsSelf();
            builder.RegisterType<PublishSampleActivityRecycService>().AsSelf();
        }
        

        #endregion Overrides of Module
    }
}