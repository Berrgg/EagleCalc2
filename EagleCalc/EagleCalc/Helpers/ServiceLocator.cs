using System;
using System.Collections.Generic;

namespace EagleCalc.Helpers
{
    public sealed class ServiceLocator
    {
        public static void Add<TContract, TService>() where TService : new()
        {
            Instance.InternalAdd<TContract, TService>();
        }

        public static T Get<T>() where T : class
        {
            return Instance.Resolve<T>();
        }

        #region Internal representation
        static readonly Lazy<ServiceLocator> instance = new Lazy<ServiceLocator>(() => new ServiceLocator());
        readonly Dictionary<Type, Lazy<object>> services = new Dictionary<Type, Lazy<object>>();

        static ServiceLocator Instance => instance.Value;

        void InternalAdd<TContract, TService>() where TService : new()
        {
            services[typeof(TContract)] = new Lazy<object>(() => Activator.CreateInstance(typeof(TService)));
        }

        T Resolve<T>() where T : class
        {
            Lazy<object> service;
            if (services.TryGetValue(typeof(T), out service))
            {
                return (T)service.Value;
            }
            return null;
        }

        #endregion
    }
}
