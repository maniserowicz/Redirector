using System.Collections.Specialized;
using System.Configuration;

namespace Procent.Redirector.Configuration
{
    public static class RedirectorConfiguration
    {
        private static NameValueCollection GetSection()
        {
            return (NameValueCollection) ConfigurationManager.GetSection("redirectorSettings");
        }

        public static string Value(string key)
        {
            return GetSection()[key];
        }
    }
}