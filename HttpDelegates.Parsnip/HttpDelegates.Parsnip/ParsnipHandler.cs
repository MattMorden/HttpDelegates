using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpDelegates.Parsnip
{
    /// <summary>
    /// Parsnip DelegatingHandler to apply the Parsnip header to outbound requests
    /// </summary>
    public class ParsnipHandler : DelegatingHandler
    {       
        /// <summary>
        /// Default Constructor for ParsnipHandler
        /// </summary>
        public ParsnipHandler() { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddParsnipHeader(request);
            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Adds the Parsnip header with a value of "YesPlease"
        /// </summary>
        /// <param name="request">HttpRequestMessage to add the Parsnip header to</param>
        private void AddParsnipHeader(HttpRequestMessage request)
        {
            if (request.Headers.Contains("Parsnip"))
            {
                request.Headers.Remove("Parsnip");
            }
            request.Headers.Add("Parsnip", "YesPlease");
        }
    }
}
