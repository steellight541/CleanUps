using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Xamarin.Essentials;

namespace CleanUps.MAUI.Shared.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        // Use MAUI SecureStorage for storing the token securely
        private readonly HttpClient _httpClient; // To potentially clear default headers on logout

        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        private const string TokenKey = "authToken";

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = await SecureStorage.GetAsync(TokenKey);
            ClaimsIdentity identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Parse the token and create the ClaimsIdentity
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

                    // Optional: Check token expiry here if needed, though API should handle it
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        // Token expired, treat as unauthenticated
                        MarkUserAsLoggedOut(); // Clean up storage
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); // Return anonymous
                    }

                    // Set default authorization header for HttpClient - IMPORTANT!
                    // This ensures subsequent API calls are authenticated.
                    // Note: This might be better handled by a DelegatingHandler for HttpClient
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

                }
                catch (Exception ex)
                {
                    // Handle token parsing errors (e.g., invalid token)
                    Console.WriteLine($"Error parsing token: {ex.Message}");
                    SecureStorage.Remove(TokenKey); // Remove invalid token
                    identity = new ClaimsIdentity(); // Ensure anonymous identity
                    _httpClient.DefaultRequestHeaders.Authorization = null; // Clear header
                }
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null; // Clear header if no token
            }

            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            await SecureStorage.SetAsync(TokenKey, token);
            ClaimsIdentity identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            // Set header immediately after login
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            SecureStorage.Remove(TokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null; // Clear header
            ClaimsIdentity identity = new ClaimsIdentity();
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        // Helper method to parse claims from JWT
        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }
}
