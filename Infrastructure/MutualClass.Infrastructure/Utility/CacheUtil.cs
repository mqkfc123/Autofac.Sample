using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Auth.Infrastructure.Utility
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        void Add<T>(string key, T obj);
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        void Add<T>(string key, T obj, TimeSpan timeSpan);
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetCacheOjbect<T>(string key);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        void RemoveCache(params string[] key);
    }
    /// <summary>
    /// 缓存依赖
    /// </summary>
    public interface ICacheDependencyService : ICacheService
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dependencyKey">依赖键</param>
        /// <param name="obj"></param>
        void Add<T>(string key, string dependencyKey, T obj);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyKey">依赖键</param>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        void Add<T>(string key, string dependencyKey, T obj, TimeSpan timeSpan);
    }
    /// <summary>
    /// 本机缓存
    /// </summary>
    public class CacheService : ICacheService
    {
        protected ObjectCache LocalCache => MemoryCache.Default;
        public void Add<T>(string key, T obj)
        {
            if (obj == null)
                return;
            var policy = new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable };
            //永久缓存
            LocalCache.Add(new CacheItem(key, obj), policy);
        }

        public void Add<T>(string key, T obj, TimeSpan timeSpan)
        {
            if (obj == null)
                return;
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + timeSpan };
            LocalCache.Add(new CacheItem(key, obj), policy);
        }

        public T GetCacheOjbect<T>(string key)
        {
            if (null == LocalCache[key]) return default(T);
            return (T)LocalCache[key];
        }

        public void RemoveCache(params string[] key)
        {
            if (!key.Any()) return;
            foreach (var k in key)
            {
                LocalCache.Remove(k);
            }
        }
    }
    /// <summary>
    /// 缓存依赖
    /// </summary>
    public class CacheDependencyService : CacheService, ICacheDependencyService
    {
        public void Add<T>(string key, string dependencyKey, T obj)
        {
            if (obj == null)
                return;
            var policy = new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable };
            LocalCache.Add(new CacheItem(key, obj), policy);
            HostFileChangeMonitor monitor = new HostFileChangeMonitor(new List<string>() { dependencyKey });
            monitor.NotifyOnChanged(new OnChangedCallback((o) =>
            {
                LocalCache.Remove(key);
            }));
            policy.ChangeMonitors.Add(monitor);
        }

        public void Add<T>(string key, string dependencyKey, T obj, TimeSpan timeSpan)
        {
            if (obj == null)
                return;
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + timeSpan };
            LocalCache.Add(new CacheItem(key, obj), policy);
            HostFileChangeMonitor monitor = new HostFileChangeMonitor(new List<string>() { dependencyKey });
            monitor.NotifyOnChanged(new OnChangedCallback((o) =>
            {
                LocalCache.Remove(key);
            }));
            policy.ChangeMonitors.Add(monitor);
        }
    }

    public class CacheUtil
    {
        public static ICacheDependencyService CacheDependency = new CacheDependencyService();
        public static ICacheService Cache = new CacheService();
    }
}
