using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using CleanUps.Shared.ClientServices.Interfaces;
using CleanUps.Shared.DTOs.Auth;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Auth API.
    /// Provides methods for performing login operations through HTTP requests.
    /// Implements error handling with Result pattern instead of throwing exceptions.
    /// </summary>
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventApiService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The factory for creating HTTP clients with preconfigured settings.</param>
        public AuthApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CleanupsApi");
        }

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="loginRequest">The login credentials.</param>
        /// <returns>
        /// A Result containing the logged-in user information if successful,
        /// a BadRequest result if the request data is invalid,
        /// an Unauthorized result if the credentials are invalid,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
                if (response.IsSuccessStatusCode)
                {
                    LoginResponse? loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    return loginResponse != null ? Result<LoginResponse>.Ok(loginResponse) : Result<LoginResponse>.InternalServerError("Failed to deserialize login response");
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<LoginResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.Unauthorized:
                        return Result<LoginResponse>.Unauthorized(errorMessage);
                    default:
                        return Result<LoginResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<LoginResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<LoginResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<LoginResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<LoginResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<Result<string>> RequestPasswordResetAsync(RequestPasswordResetRequest request)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/request-password-reset", request);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Result<string>.Ok(responseContent); // Return the success message from API
                }
                else
                {
                    // Use static methods for error results
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            return Result<string>.BadRequest(responseContent);
                        // Add other specific status codes if needed
                        default:
                            // Use InternalServerError or a more specific status based on API contract
                            return Result<string>.InternalServerError(responseContent);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<string>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<string>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ValidateResetTokenAsync(ValidateTokenRequest request)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/validate-reset-token", request);

                if (response.IsSuccessStatusCode)
                {
                    return Result<bool>.Ok(true);
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                 switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<bool>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<bool>.NotFound(errorMessage);
                    case HttpStatusCode.Conflict:
                        return Result<bool>.Conflict(errorMessage);
                    default:
                        return Result<bool>.InternalServerError(errorMessage);
                }
            }
             catch (HttpRequestException ex)
            {
                return Result<bool>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<bool>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
             try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
                string responseContent = await response.Content.ReadAsStringAsync();

                 if (response.IsSuccessStatusCode)
                {
                     return Result<string>.Ok(responseContent); // Return the success message from API
                }
                else
                {
                     // Use static methods for error results
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            return Result<string>.BadRequest(responseContent);
                        case HttpStatusCode.NotFound:
                            return Result<string>.NotFound(responseContent);
                        case HttpStatusCode.Conflict:
                            return Result<string>.Conflict(responseContent);
                        // Add other specific status codes if needed
                        default:
                            // Use InternalServerError or a more specific status based on API contract
                            return Result<string>.InternalServerError(responseContent);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<string>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<string>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}
