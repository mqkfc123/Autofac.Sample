using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;

namespace Dragon.Core
{
    public sealed class CoreBuilder : ICoreBuilder
    {
        public ContainerBuilder builder { get; set; }

        private readonly IDictionary<string, Delegate> _actionDictionary = new Dictionary<string, Delegate>();
        private int _actionIdentity;
        public CoreBuilder()
        {
            builder = new ContainerBuilder(); 
        }

        public IContainer Build()
        {
            var configuration = GlobalConfiguration.Configuration;
            Type baseType = typeof(IDependency);
            // 获取所有相关类库的程序集
            //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly[] assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();

            using (IEnumerator<Action<ContainerBuilder>> enumerator = this.GetBuildingActions().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current(builder);
                }
            }

            //注册mvc conrollers
            builder.RegisterControllers(assemblies);
            //注册api conrollers
            builder.RegisterApiControllers(assemblies); 

            builder.RegisterAssemblyTypes(assemblies)
                   .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();  //InstancePerLifetimeScope 保证对象生命周期基于请求
            IContainer container = builder.Build(0);

            //注册api容器需要使用HttpConfiguration对象
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container); 
            //注册mvc容器 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            CoreBuilderWork.LifetimeScope = container;
            return container;
        }


        public IContainer Build(params Assembly[] assemblies)
        {
            Type baseType = typeof(IDependency);
            using (IEnumerator<Action<ContainerBuilder>> enumerator = this.GetBuildingActions().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current(builder);
                }
            }
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();  //InstancePerLifetimeScope 保证对象生命周期基于请求
            IContainer container = builder.Build(0);
            CoreBuilderWork.LifetimeScope = container;
            return container;
        }
         
        
        public ICoreBuilder OnStarted(Action<IContainer> action, string actionName = null)
        {
            string str = this.GetActionName(actionName);
            this._actionDictionary[str] = action;
            return this;
        } 

        public ICoreBuilder OnStarting(Action<ContainerBuilder> action, string actionName = null)
        {
            string str = this.GetActionName(actionName);
            this._actionDictionary[str] = action;
            return this;
        }

        private string GetActionName(string actionName)
        {
            if (!string.IsNullOrWhiteSpace(actionName))
            {
                return string.Concat("Custom_", actionName);
            }
            this._actionIdentity = this._actionIdentity + 1;
            return string.Concat("Default_", this._actionIdentity);
        }

        private IEnumerable<Action<IContainer>> GetBuildedActions()
        {
            return (
                from i in this._actionDictionary
                where i.Value is Action<IContainer>
                select i.Value).OfType<Action<IContainer>>().ToArray<Action<IContainer>>();
        }

        private IEnumerable<Action<ContainerBuilder>> GetBuildingActions()
        {
            return (
                from i in this._actionDictionary
                where i.Value is Action<ContainerBuilder>
                select i.Value).OfType<Action<ContainerBuilder>>().ToArray<Action<ContainerBuilder>>();
        }

    }

    public sealed class CoreBuilderWork
    {
        public static ILifetimeScope LifetimeScope { get; set; }
    }
}