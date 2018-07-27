using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutualClass.WindowsService.Consumer.Services
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
            builder.RegisterType<MutualClassRewardQueueService>().AsSelf();  
        }
        
        #endregion Overrides of Module
    }
}
