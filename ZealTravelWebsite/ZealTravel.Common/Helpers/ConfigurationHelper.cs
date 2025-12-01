using Microsoft.Extensions.Configuration;

namespace ZealTravel.Common.Helpers
{

    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetSetting(string key)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration has not been initialized. Call Initialize method first.");
            }
            return _configuration[key];
        }
        public static string GetConnectionString(string name)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration has not been initialized. Call Initialize method first.");
            }
            return _configuration.GetConnectionString(name);
        }
    }
}
