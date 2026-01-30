using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace PetCare.MobileApp.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_currentUser.Identity != null && _currentUser.Identity.IsAuthenticated)
            {
                return new AuthenticationState(_currentUser);
            }

            try
            {
                var token = await SecureStorage.GetAsync("auth_token");

                if (!string.IsNullOrEmpty(token))
                {
                    var claims = ParseClaimsFromJwt(token);
                    var identity = new ClaimsIdentity(claims, "apiauth");

                    var expClaim = identity.FindFirst(c => c.Type == "exp");

                    if (expClaim != null)
                    {
                        var expSeconds = long.Parse(expClaim.Value);
                        var expDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

                        if (expDate < DateTime.UtcNow)
                        {
                            SecureStorage.Remove("auth_token");
                            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                            return new AuthenticationState(_currentUser);
                        }
                    }

                    _currentUser = new ClaimsPrincipal(identity);
                }
                else
                {
                    _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                }
            }
            catch
            {
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            }

            return new AuthenticationState(_currentUser);
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var claims = ParseClaimsFromJwt(token);

            var identity = new ClaimsIdentity(claims, "apiauth");

            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void MarkUserAsLoggedOut()
        {
            SecureStorage.Remove("auth_token");
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claimMapping = new Dictionary<string, string>
            {
                { "unique_name", ClaimTypes.Name },
                { "name", ClaimTypes.Name },
                { "role", ClaimTypes.Role },
            };

            var claims = new List<Claim>();

            foreach (var kvp in keyValuePairs)
            {
                var value = kvp.Value.ToString();
                var claimType = kvp.Key;

                if (claimMapping.TryGetValue(kvp.Key, out var mappedType))
                {
                    claimType = mappedType;
                }

                claims.Add(new Claim(claimType, value));
            }

            return claims;
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