using Hanssens.Net;
using System.Net.Http.Headers;

namespace Client.Handlers
{
    public class JwtHandler : DelegatingHandler
    {
        private readonly ILocalStorage _localStorage;

        public JwtHandler(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Options.TryGetValue(new HttpRequestOptionsKey<bool>("Authorize"), out var authorize) && authorize)
            {
                var token = _localStorage.Get<string>("authToken");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}