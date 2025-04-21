using CleanUps.Shared.ErrorHandling;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using CleanUps.Shared.ClientServices.Interfaces;
using CleanUps.Shared.DTOs.Auth;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Authentication API endpoints.
    /// Provides methods for user authentication, password reset, and token validation.
    /// Implements standardized error handling using the Result pattern for consistent response handling.
    /// </summary>
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthApiService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The factory for creating HTTP clients with preconfigured settings.</param>
        public AuthApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CleanupsApi");
        }

        /// <summary>
        /// Authenticates a user with the provided credentials and retrieves their profile information.
        /// </summary>
        /// <param name="loginRequest">The login credentials containing email and password.</param>
        /// <returns>
        /// A Result containing the authenticated user information and access token if successful,
        /// a BadRequest result if the credentials format is invalid,
        /// an Unauthorized result if the credentials are incorrect,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                // Step 1: Send the login request to the API endpoint
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the successful response
                    LoginResponse? loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    return loginResponse != null 
                        ? Result<LoginResponse>.Ok(loginResponse) 
                        : Result<LoginResponse>.InternalServerError("Failed to deserialize login response");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<LoginResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON parsing exceptions
                return Result<LoginResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Step 7: Handle request timeout exceptions
                return Result<LoginResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<LoginResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Initiates a password reset request for a user by sending an email with reset instructions.
        /// </summary>
        /// <param name="request">The password reset request containing the user's email address.</param>
        /// <returns>
        /// A Result containing a success message if the request was processed,
        /// a BadRequest result if the email format is invalid,
        /// or an error message if the request fails or the email is not found.
        /// </returns>
        public async Task<Result<string>> RequestPasswordResetAsync(EmailPasswordResetRequest request)
        {
            try
            {
                // Step 1: Send the password reset request to the API
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/request-password-reset", request);
                
                // Step 2: Read the response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Step 3: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    return Result<string>.Ok(responseContent); // Return the success message from API
                }
                else
                {
                    // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<string>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 6: Handle any other unexpected exceptions
                return Result<string>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates a password reset token to ensure it's valid and not expired.
        /// </summary>
        /// <param name="request">The token validation request containing the token to verify.</param>
        /// <returns>
        /// A Result with a true value if the token is valid,
        /// a BadRequest result if the token format is invalid,
        /// a NotFound result if the token doesn't exist,
        /// a Conflict result if the token is expired or already used,
        /// or an error message if the validation fails.
        /// </returns>
        public async Task<Result<bool>> ValidateResetTokenAsync(ValidateTokenRequest request)
        {
            try
            {
                // Step 1: Send the token validation request to the API
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/validate-reset-token", request);

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    return Result<bool>.Ok(true);
                }

                // Step 3: Handle error responses based on status code
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
                // Step 4: Handle network-related exceptions
                return Result<bool>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 5: Handle any other unexpected exceptions
                return Result<bool>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Resets a user's password using a valid reset token.
        /// </summary>
        /// <param name="request">The password reset request containing the token, new password, and confirmation.</param>
        /// <returns>
        /// A Result containing a success message if the password was reset,
        /// a BadRequest result if the request format is invalid (e.g., password mismatch),
        /// a NotFound result if the token or user doesn't exist,
        /// a Conflict result if the token is expired or already used,
        /// or an error message if the reset operation fails.
        /// </returns>
        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
             try
            {
                // Step 1: Send the password reset request to the API
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
                
                // Step 2: Read the response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Step 3: Process successful response
                if (response.IsSuccessStatusCode)
                {
                     return Result<string>.Ok(responseContent); // Return the success message from API
                }
                else
                {
                    // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<string>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 6: Handle any other unexpected exceptions
                return Result<string>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}
