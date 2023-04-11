using System.Configuration;

namespace CosmosAPI.Models
{
    public class AppSettings
    {

        public LoggingSection Logging { get; set; }
        public string AllowedHosts { get; set; }
        public ConnectionStringsSection ConnectionStrings { get; set; }
    }

    public class LoggingSection
    {
        public LogLevelSection LogLevel { get; set; }
    }

    public class LogLevelSection
    {
        public string Default { get; set; }
        public string MicrosoftAspNetCore { get; set; }
    }

    public class ConnectionStringsSection
    {
        public string CosmosAPIContext { get; set; }
        public string CosmosEndpoint { get; set; }
        public string ApplicationName { get; set; }
        public string CosmosKey { get; set; }

    }
}
