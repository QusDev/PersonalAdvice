using Client.Constant;
using Client.Services.Interfaces;
using Microsoft.Extensions.Options;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.UserInteractions;
using Shared.Enums;

namespace Client.Services
{
    public class UserInteractionService : IUserInteractionService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _options;

        public UserInteractionService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<(bool isSuccess, string message)> DeleteUserInteractionAsync(int id)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/user-interactions?id={id}";
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
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

        public async Task HandleUserInteraction(UserInteractionDto dto)
        {
            dto.Weight = dto.Type switch
            {
                UserInteractionType.Like => 10,
                UserInteractionType.Dislike => -10,
                UserInteractionType.View => 2,
                UserInteractionType.Listen => 3,
                _ => 0
            };

            var url = $"{_options.Value.BackendApiBaseUrl}/user-interactions/handle";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Options.Set(new HttpRequestOptionsKey<bool>("Authorize"), true);
            request.Content = JsonContent.Create(dto);
            var response = await _httpClient.SendAsync(request);
        }

        public async Task<(UserInteractionDto? userInteraction, string message)> GetUserInteraction(int mediaId, int userId)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/user-interactions/media-user?mediaId={mediaId}&userId={userId}";
            var request = await _httpClient.GetAsync(url);

            if (request.IsSuccessStatusCode)
            {
                return (await request.Content.ReadFromJsonAsync<UserInteractionDto>(), "");
            }

            try
            {
                var errors = await request.Content.ReadFromJsonAsync<List<Error>>();
                var message = string.Join("\n", errors!.Select(x => x.Message));
                return (null, message);
            }
            catch (Exception)
            {
                return (null, "invalid connection");
            }
        }
    }
}
