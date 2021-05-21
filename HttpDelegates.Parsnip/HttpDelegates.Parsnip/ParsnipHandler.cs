using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpDelegates.Parsnip
{
    public class ParsnipHandler : DelegatingHandler
    {       
        public ParsnipHandler() { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddParsnipHeader(request);
            return await base.SendAsync(request, cancellationToken);
        }

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
