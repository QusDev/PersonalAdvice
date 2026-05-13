using Client.Constant;
using Client.Services.Interfaces;
using Microsoft.Extensions.Options;
using Shared;
using Shared.DTOs.Entities;
using Shared.DTOs.Movie;
using Shared.DTOs.Repositories;

namespace Client.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _options;

        public MovieService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<(bool isSuccess, string message)> CreateMovieAsync(CreateMovieDto dto)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/movies";

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

        public async Task<(bool isSuccess, string message)> DeleteMovieAsync(int id)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/movies?id={id}";
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

        public async Task<PagedResponse<MovieDto>?> GetMoviesAsync(GetAllMovieDto dto)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/movies?pageNumber={dto.PageNumber}&pageSize={dto.PageSize}";
            var response = await _httpClient.GetFromJsonAsync<PagedResponse<MovieDto>>(url);
            return response;
        }

        public async Task<(bool isSuccess, string message)> ImportFromTmdbAsync(int pageNumber, int pageCount)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/movies/import/popular?page={pageNumber}&pageCount={pageCount}";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
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

        public async Task<(bool isSuccess, string message)> UpdateMovieAsync(UpdateMovieDto dto)
        {
            var url = $"{_options.Value.BackendApiBaseUrl}/movies";
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
    }
}
