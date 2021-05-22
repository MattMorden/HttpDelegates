using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpDelegatesTester
{
    /// <summary>
    /// Test program to verify DelegatingHandlers
    /// </summary>
    class Program
    {
        private static readonly Startup _startup;
        private static readonly IServiceCollection _serviceCollection;
        private static readonly IServiceProvider _serviceProvider;
        private static readonly IHttpClientFactory _httpClientFactory;

        static Program()
        {
            // Instantiate a new ServiceCollection
            _serviceCollection = new ServiceCollection();

            // Create a new Startup object to retrieve dependencies
            _startup = new Startup();
            _startup.ConfigureServices(_serviceCollection);
            _serviceProvider = _serviceCollection.BuildServiceProvider();

            _httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        }

        static async Task Main(string[] args)
        {
            await GetTestApiContent();
        }

        private static async Task GetTestApiContent()
        {
            using (var client = _httpClientFactory.CreateClient("Test"))
            {
                var req = new HttpRequestMessage(HttpMethod.Get, "/todos/1");
                var res = await client.SendAsync(req);
                var ret = await res.Content.ReadAsStringAsync();
                Console.WriteLine(ret);
            }
        }
    }
}
