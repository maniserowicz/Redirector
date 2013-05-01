using System;
using System.Web.Http;
using Raven.Client;

namespace Procent.Redirector.API
{
    public class RedirectorControllerBase : ApiController
    {
        public Func<IDocumentSession> NewSession;
    }
}