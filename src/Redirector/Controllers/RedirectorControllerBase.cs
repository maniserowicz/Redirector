using System;
using System.Web.Http;
using Raven.Client;

namespace Procent.Redirector.API
{
    public abstract class RedirectorControllerBase : ApiController
    {
        public Func<IDocumentSession> NewSession;
    }
}