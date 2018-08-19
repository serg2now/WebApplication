using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Unity;

namespace WebApplicationExercise.Core
{
    public class UnityResolver : IDependencyResolver
    {
        private IUnityContainer container;

        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            object service;

            try
            {
                service = container.Resolve(serviceType);
            }
            catch (Exception)
            {
                service = null;
            }

            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IEnumerable<object> services;

            try
            {
                services = container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                services = null;
            }

            return services;
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
}