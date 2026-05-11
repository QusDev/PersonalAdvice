using Client.Constant;
using Client.Providers;
using Client.Services.Interfaces;
using Hanssens.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Shared;
using Shared.DTOs.Identity;
using System.Net.Http.Headers;

namespace Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorage _localStorage;
        private readonly IOptions<ApiSettings> _options;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorage localStorage, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _options = options;
        }

        public async Task<(bool isSuccess, string message)> LoginAsync(LoginDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_options.Value.BackendApiBaseUrl}/auth/login", dto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                _localStorage.Store("authToken", result.Token);
                _localStorage.Persist();

                ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogin(result.Token);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
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

        public async Task LogoutAsync()
        {
            _localStorage.Remove("authToken");
            _localStorage.Persist();

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<(bool IsSuccess, string message)> RegisterAsync(RegisterDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_options.Value.BackendApiBaseUrl}/auth/register", dto);

            if (response.IsSuccessStatusCode)
            {
                return (true, "");
            }

            try
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<List<Error>>();
                var message = string.Join("\n", errorResponse!.Select(x => x.Message));
                return (false, message);
            }
            catch (Exception)
            {
                return (false, "invalid connection");
            }
        }
    }
}
