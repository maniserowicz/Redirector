using System;
using System.Web.Http;
using Raven.Client;

namespace Procent.Redirector.API
{
    public class RedirectorControllerBase : ApiController
    {
        protected readonly Func<IDocumentSession> _newSession;

        public RedirectorControllerBase(Func<IDocumentSession> newSession)
        {
            _newSession = newSession;
        }
    }
}