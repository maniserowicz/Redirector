using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;

namespace Procent.Redirector.API
{
    public class LinksController : RedirectorControllerBase
    {
        public LinksController(Func<IDocumentSession> newSession)
            : base(newSession)
        {
        }

        public IEnumerable<Link> Get()
        {
            using (var session = _newSession())
            {
                return session.Query<Link>().ToList();
            }
        }
    }
}