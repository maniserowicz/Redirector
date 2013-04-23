using System.Collections.Generic;

namespace Procent.Redirector
{
    public class Link
    {
        public string Id { get; set; }
        public string Alias { get; set; }
        public string Target { get; set; }
        public List<Visit> Visits { get; set; }

        public Link()
        {
            Visits = new List<Visit>();
        }
    }
}