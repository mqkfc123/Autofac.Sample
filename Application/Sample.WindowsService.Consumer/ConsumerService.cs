using MutualClass.WindowsService.Consumer.Job;
using MutualClass.WindowsService.Consumer.Services;
using MutualClass.WindowsService.Consumer.Utility;
using Autofac;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Dragon.Core.Log4net;
using Dragon.Core;
using Dragon.Infrastructure.Services;
using Dragon.Infrastructure.Redis;

namespace MutualClass.WindowsService.Consumer
{
    public partial class ConsumerService : ServiceBase
    {

        private readonly ILog _Logger = LogHelper.GetLogger(typeof(ConsumerService));

        public ConsumerService()
        {
            try
            {
                var coreBuilder = new CoreBuilder();
                coreBuilder.builder.RegisterModule<AutofacModules>();
                coreBuilder.UserRedis(ConfigurationManager.ConnectionStrings["RedisConnectionString"].ConnectionString);

                coreBuilder.OnStarting(builder =>
                {
                    builder.RegisterType<MemoryEventBusService>().As<IMemoryEventBusService>().InstancePerMatchingLifetimeScope();
                    builder.RegisterType<DistributedEventBusService>().As<IDistributedEventBusService>().InstancePerMatchingLifetimeScope();
                });

                //拆分DLL后需要注册，需要注入的DLL
                Assembly[] asm = Utils.GetAllAssembly("MutualClass.*.dll").ToArray();
                coreBuilder.Build(asm);
                CoreBuilderWork.LifetimeScope.Resolve<IDistributedEventBusService>();

                //var _MutualClassRewardQueueService = CoreBuilderWork.LifetimeScope.Resolve<MutualClassRewardQueueService>();
                //_MutualClassRewardQueueService.imq_onReceive_test();

            }
            catch (Exception ex)
            {
                _Logger.Error("Autofac注册异常", ex);
            }
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _Logger.Info("服务启动成功");
            try
            {
                //RabbitMQ 订阅  
                var MutualClassRewardQueueService = CoreBuilderWork.LifetimeScope.Resolve<MutualClassRewardQueueService>();
                MutualClassRewardQueueService.InitMessRabbitMQ();
            }
            catch (Exception ex)
            {
                _Logger.Error("服务启动失败 OnStart ：", ex);
            }

        }

        protected override void OnStop()
        {  
            _Logger.Info("服务停止");
        }
    }
}
