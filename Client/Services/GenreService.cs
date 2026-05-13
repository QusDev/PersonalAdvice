using Client.Constant;
using Client.Services.Interfaces;
using Microsoft.Extensions.Options;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Genres;
using Shared.DTOs.Repositories;

namespace Client.Services
{
    public class GenreService : IGenreService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _options;

        public GenreService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<PagedResponse<GenreDto>?> GetGenresAsync(GetAllGenreDto dto)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/genres?pageNumber={dto.PageNumber}&pageSize={dto.PageSize}";
            var response = await _httpClient.GetFromJsonAsync<PagedResponse<GenreDto>>(url);
            return response;
        }

        public async Task<(bool isSuccess, string message)> CreateGenreAsync(CreateGenreDto dto)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/genres";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Options.Set(new HttpRequestOptionsKey<bool>("Authorize"), true);
            request.Content = JsonContent.Create(dto);
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

        public async Task<(bool isSuccess, string message)> UpdateGenreAsync(UpdateGenreDto dto)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/genres";
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Options.Set(new HttpRequestOptionsKey<bool>("Authorize"), true);
            request.Content = JsonContent.Create(dto);
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

        public async Task<(bool isSuccess, string message)> DeleteGenreAsync(int id)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/genres?id={id}";
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
    }
}
