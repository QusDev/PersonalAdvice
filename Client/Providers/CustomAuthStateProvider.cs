using Microsoft.AspNetCore.Components.Authorization;
using Hanssens.Net;
using System.Security.Claims;
using System.Text.Json;

namespace Client.Providers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly  ILocalStorage _localStorage;
        private readonly ClaimsPrincipal _anonymous;

        public CustomAuthStateProvider(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
            _anonymous = new(new ClaimsIdentity());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!_localStorage.Exists("authToken"))
                return new AuthenticationState(_anonymous);

            var token = _localStorage.Get<string>("authToken");

            if (string.IsNullOrWhiteSpace(token))
                return new AuthenticationState(_anonymous);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        public void NotifyUserLogin(string token)
        {
            _localStorage.Store("authToken", token);
            _localStorage.Persist();

            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            _localStorage.Remove("authToken");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
