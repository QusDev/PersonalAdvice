using Client.Constant;
using Client.Services.Interfaces;
using Microsoft.Extensions.Options;
using Shared.DTOs.Entities;
using Shared.DTOs.MediaContent;
using Shared.DTOs.Repositories;

namespace Client.Services
{
    public class MediaService : IMediaService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _options;

        public MediaService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<PagedResponse<MediaCardDto>> GetUserMediaRecommendationsAsync(GetUserMediaRecommendationsDto dto)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/media/recommendations?pageNumber={dto.PageNumber}&pageSize={dto.PageSize}&userId={dto.UserId}&algorithm={dto.Algorithm}";
            var response = await _httpClient.GetFromJsonAsync<PagedResponse<MediaCardDto>>(url);
            return response;
        }
    }
}
