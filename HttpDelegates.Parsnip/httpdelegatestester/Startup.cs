using HttpDelegates.Wiki;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace HttpDelegatesTester
{
    /// <summary>
    /// Startup Class. Handles registration of the default Configuration and Dependency Injection
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The ConfigurationRoot
        /// </summary>
        IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Default constructor for Startup
        /// </summary>
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        /// <summary>
        /// Configures the IServiceCollection with service dependencies
        /// </summary>
        /// <param name="serviceCollection">IServiceCollection dependency to register services against</param>
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient("Wiki", c => {
                c.BaseAddress = new Uri("https://en.wikipedia.org/wiki");
            });

            serviceCollection.AddHttpClient("Test", c =>
            {
                c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            }).AddHttpMessageHandler(c => { 
                return new Wiki(c.GetRequiredService<IHttpClientFactory>()); 
            });

            serviceCollection.AddSingleton<IConfigurationRoot>(Configuration);
        }
        
    }
}
