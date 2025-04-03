using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;
using System.Net.Http.Json;
using System.Net;

namespace CleanUps.Shared.ClientServices
{
    public class AuthApiService
    {
        private readonly HttpClient _http;

        public AuthApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Result<LoginResponseDTO>> LoginAsync(LoginRequestDTO credentials)
        {
            try
            {
                HttpResponseMessage response = await _http.PostAsJsonAsync("api/auth/login", credentials);

                if (response.IsSuccessStatusCode)
                {
                    LoginResponseDTO? loginResult = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                    return loginResult != null ? Result<LoginResponseDTO>.Ok(loginResult) : Result<LoginResponseDTO>.InternalServerError("Failed to deserialize login response.");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(errorContent))
                    {
                        errorContent = "Unknown login error";
                    }
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            return Result<LoginResponseDTO>.Unauthorized(errorContent);
                        case HttpStatusCode.BadRequest:
                            return Result<LoginResponseDTO>.BadRequest(errorContent);
                        default:
                            return Result<LoginResponseDTO>.InternalServerError($"Login failed: {response.StatusCode}. Details: {errorContent}");
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Network error, server unreachable etc.
                return Result<LoginResponseDTO>.InternalServerError($"Network error during login: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Other unexpected errors
                return Result<LoginResponseDTO>.InternalServerError($"An unexpected error occurred during login: {ex.Message}");
            }
        }
    }
}
