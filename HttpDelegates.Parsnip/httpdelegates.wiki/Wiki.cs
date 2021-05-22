using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpDelegates.Wiki
{
    /// <summary>
    /// Wiki DelegatingHandler to apply the X-Parsnip header to outbound requests
    /// </summary>
    public class Wiki : DelegatingHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _httpClientFactoryName = "";
        private readonly string _wikiUri = "/wiki/Parsnip";

        /// <summary>
        /// Default constructor for Wiki
        /// </summary>
        /// <param name="httpClientFactory">httpClientFactory dependency</param>
        /// <param name="httpClientFactoryName">httpClientFactoryName dependency</param>
        public Wiki(IHttpClientFactory httpClientFactory, string httpClientFactoryName = "Wiki")
        {
            _httpClientFactory = httpClientFactory;
            _httpClientFactoryName = httpClientFactoryName ?? "Wiki";
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
                request.Headers.Add("Accept", "application/json");
                string externalParsnip = await GetExternalParsnip();
                request.Headers.Add("X-Parsnip", externalParsnip);

                return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Retrieves the value of the X-Parsnip header to apply.
        /// </summary>
        /// <remarks>
        /// If the request to the Parsnip wikipedia page returns successful, apply "Parsnip"
        /// If the request to the Parsnip wikipedia page is not successful, apply "No-Parsnip"
        /// If an exception is thrown, apply "failed"
        /// </remarks>
        /// <returns>Task<string></returns>
        private async Task<string> GetExternalParsnip()
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient(_httpClientFactoryName))
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, _wikiUri);
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string resString = await response.Content.ReadAsStringAsync();
                        return "Parsnip";
                    }
                    return "No-Parsnip";
                }
            }
            catch (Exception ex)
            {
                return "failed";
            }
        }
    }
}
