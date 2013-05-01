using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Procent.Redirector.API;
using Raven.Client;

namespace Procent.Redirector.Configuration.DI
{
    // copy/paste from http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
    // A simple implementation of IDependencyResolver, for example purposes.
    public class SimpleContainer : IDependencyResolver
    {
        static readonly Func<IDocumentSession> _openSession = RavenStore.Session;

        public IDependencyScope BeginScope()
        {
            // This example does not support child scopes, so we simply return 'this'.
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (typeof (RedirectorControllerBase).IsAssignableFrom(serviceType))
            {
                var controller = (RedirectorControllerBase)Activator.CreateInstance(serviceType);
                controller.NewSession = _openSession;
                return controller;
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
        }
    }
}