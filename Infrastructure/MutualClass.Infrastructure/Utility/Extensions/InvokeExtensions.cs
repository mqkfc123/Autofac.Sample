using Dragon.Core.Log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Infrastructure.Utility.Extensions
{
    public static class InvokeExtensions
    {
        private static readonly ILog _Logger = LogHelper.GetLogger(typeof(InvokeExtensions));

        public static void Invoke<TEvents>(this IEnumerable<TEvents> events, Action<TEvents> dispatch)
        {
            foreach (TEvents current in events)
            {
                try
                {
                    dispatch(current);
                }
                catch (Exception ex)
                {
                    if (InvokeExtensions.IsLogged(ex))
                    {
                        _Logger.Error($"由 {typeof(TEvents).Name} 抛出来自 {current.GetType().FullName} 的异常：{ex.GetType().Name}", ex);
                    }
                    if (ex.IsFatal())
                    {
                        throw;
                    }
                }
            }
        }
        public static IEnumerable<TResult> Invoke<TEvents, TResult>(this IEnumerable<TEvents> events, Func<TEvents, TResult> dispatch)
        {
            foreach (var sink in events)
            {
                TResult result = default(TResult);
                try
                {
                    result = dispatch(sink);
                }
                catch (Exception ex)
                {
                    if (IsLogged(ex))
                    {
                        _Logger.Error($"由 {typeof(TEvents).Name} 抛出来自 {sink.GetType().FullName} 的异常：{ex.GetType().Name}", ex);
                    }
                    if (ex.IsFatal())
                    {
                        throw;
                    }
                }
                yield return result;
            }
        }

        private static bool IsLogged(Exception ex)
        {
            return !ex.IsFatal();
        }
    }

}
