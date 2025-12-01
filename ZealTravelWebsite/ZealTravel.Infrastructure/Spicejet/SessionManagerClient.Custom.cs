using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Extensions.Configuration;

namespace svc_session
{
    public partial class SessionManagerClient
    {
        static partial void ConfigureEndpoint(ServiceEndpoint serviceEndpoint, ClientCredentials clientCredentials)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // <- needs Microsoft.Extensions.Configuration.FileExtensions
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var url = config["SpicejetSoapService:SessionmanagerURL"];
            if (!string.IsNullOrWhiteSpace(url))
            {
                serviceEndpoint.Address = new EndpointAddress(url);
            }
        }
    }
}