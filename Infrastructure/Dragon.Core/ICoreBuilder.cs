using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Core
{
    /// <summary>
    /// 摘要:
    /// 一个抽象的内核建设者。
    /// </summary>
    public interface ICoreBuilder
    {
        IContainer Build();

        ICoreBuilder OnStarted(Action<IContainer> action, string actionName = null);

        ICoreBuilder OnStarting(Action<ContainerBuilder> action, string actionName = null);

    }
}
