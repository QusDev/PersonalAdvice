using Client.Constant;
using Client.Services.Interfaces;
using Microsoft.Extensions.Options;
using Shared;

namespace Client.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _options;

        public RecommendationService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<(bool isSuccess, string message)> UpdateCollaborativeRecs(int userId)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/recommendations/collaborative?userId={userId}";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Options.Set(new HttpRequestOptionsKey<bool>("Authorize"), true);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return (true, "");
            }

            try
            {
                var errors = await response.Content.ReadFromJsonAsync<List<Error>>();
                var message = string.Join("\n", errors!.Select(x => x.Message));
                return (false, message);
            }
            catch (Exception)
            {
                return (false, "invalid connection");
            }
        }

        public async Task<(bool isSuccess, string message)> UpdateContentBasedRecs(int userId)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/recommendations/content-based?userId={userId}";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Options.Set(new HttpRequestOptionsKey<bool>("Authorize"), true);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return (true, "");
            }

            try
            {
                var errors = await response.Content.ReadFromJsonAsync<List<Error>>();
                var message = string.Join("\n", errors!.Select(x => x.Message));
                return (false, message);
            }
            catch (Exception)
            {
                return (false, "invalid connection");
            }
        }

        public async Task<(bool isSuccess, string message)> UpdateHybridRecs(int userId)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/recommendations/hybrid?userId={userId}";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Options.Set(new HttpRequestOptionsKey<bool>("Authorize"), true);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return (true, "");
            }

            try
            {
                var errors = await response.Content.ReadFromJsonAsync<List<Error>>();
                var message = string.Join("\n", errors!.Select(x => x.Message));
                return (false, message);
            }
            catch (Exception)
            {
                return (false, "invalid connection");
            }
        }
    }
}
