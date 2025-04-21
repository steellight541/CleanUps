using CleanUps.Shared.ClientServices.Interfaces;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.DTOs.Auth;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Users API endpoints.
    /// Provides methods for performing CRUD operations on users through HTTP requests.
    /// Implements standardized error handling using the Result pattern for consistent response handling.
    /// </summary>
    public class UserApiService : IUserApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserApiService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The factory for creating HTTP clients with preconfigured settings.</param>
        public UserApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CleanupsApi");
        }

        /// <summary>
        /// Retrieves all users from the API endpoints.
        /// </summary>
        /// <returns>
        /// A Result containing a list of all users if successful,
        /// a NoContent result if no users exist,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            try
            {
                // Step 1: Send request to the API endpoint to retrieve all users
                HttpResponseMessage response = await _httpClient.GetAsync("api/users");
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the successful response
                    List<UserResponse>? users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
                    return users != null 
                        ? Result<List<UserResponse>>.Ok(users) 
                        : Result<List<UserResponse>>.InternalServerError("Failed to deserialize users");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<List<UserResponse>>.BadRequest(errorMessage);
                    case HttpStatusCode.NoContent:
                        return Result<List<UserResponse>>.NoContent();
                    default:
                        return Result<List<UserResponse>>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<List<UserResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Step 6: Handle request timeout exceptions
                return Result<List<UserResponse>>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<List<UserResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>
        /// A Result containing the requested user if found,
        /// a NotFound result if the user doesn't exist,
        /// a BadRequest result if the ID is invalid,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<UserResponse>> GetByIdAsync(int id)
        {
            try
            {
                // Step 1: Send request to the API endpoint to retrieve specific user
                HttpResponseMessage response = await _httpClient.GetAsync($"api/users/{id}");
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the successful response
                    UserResponse? user = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return user != null 
                        ? Result<UserResponse>.Ok(user) 
                        : Result<UserResponse>.InternalServerError("Failed to deserialize user");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<UserResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<UserResponse>.NotFound(errorMessage);
                    default:
                        return Result<UserResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<UserResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Step 6: Handle request timeout exceptions
                return Result<UserResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<UserResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new user through the API.
        /// </summary>
        /// <param name="createRequest">The data for creating a new user.</param>
        /// <returns>
        /// A Result containing the created user if successful,
        /// a BadRequest result if the request data is invalid,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest createRequest)
        {
            try
            {
                // Step 1: Send create request to the API endpoint
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/users", createRequest);
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the created user from the response
                    UserResponse? createdUser = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return createdUser != null 
                        ? Result<UserResponse>.Created(createdUser) 
                        : Result<UserResponse>.InternalServerError("Failed to deserialize user");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<UserResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<UserResponse>.NotFound(errorMessage);
                    default:
                        return Result<UserResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<UserResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON parsing exceptions
                return Result<UserResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Step 7: Handle request timeout exceptions
                return Result<UserResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<UserResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user through the API.
        /// </summary>
        /// <param name="updateRequest">The updated user data, including the user ID.</param>
        /// <returns>
        /// A Result containing the updated user if successful,
        /// a BadRequest result if the request data is invalid,
        /// a NotFound result if the user doesn't exist,
        /// a Conflict result if there's a concurrency issue or email conflict,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<UserResponse>> UpdateAsync(UpdateUserRequest updateRequest)
        {
            try
            {
                // Step 1: Send update request to the API endpoint
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/users/{updateRequest.UserId}", updateRequest);
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the updated user from the response
                    UserResponse? updatedUser = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return updatedUser != null 
                        ? Result<UserResponse>.Ok(updatedUser) 
                        : Result<UserResponse>.InternalServerError("Failed to deserialize user");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<UserResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<UserResponse>.NotFound(errorMessage);
                    case HttpStatusCode.Conflict:
                        return Result<UserResponse>.Conflict(errorMessage);
                    default:
                        return Result<UserResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<UserResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON parsing exceptions
                return Result<UserResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Step 7: Handle request timeout exceptions
                return Result<UserResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<UserResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a user through the API.
        /// </summary>
        /// <param name="deleteRequest">The request containing the ID of the user to delete.</param>
        /// <returns>
        /// A Result containing the deleted user if successful,
        /// a BadRequest result if the ID is invalid,
        /// a NotFound result if the user doesn't exist,
        /// a Conflict result if the user cannot be deleted,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<UserResponse>> DeleteAsync(DeleteUserRequest deleteRequest)
        {
            try
            {
                // Step 1: Send delete request to the API endpoint
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/users/{deleteRequest.Id}");
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the deleted user from response
                    UserResponse? deletedUser = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return deletedUser != null 
                        ? Result<UserResponse>.Ok(deletedUser) 
                        : Result<UserResponse>.InternalServerError("Failed to deserialize user");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<UserResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<UserResponse>.NotFound(errorMessage);
                    case HttpStatusCode.Conflict:
                        return Result<UserResponse>.Conflict(errorMessage);
                    default:
                        return Result<UserResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<UserResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Step 6: Handle request timeout exceptions
                return Result<UserResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<UserResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a request to the API to change the user's password.
        /// </summary>
        /// <param name="changeRequest">The password change request details.</param>
        /// <returns>
        /// A Result indicating success or failure of the password change operation,
        /// a BadRequest result if the request is invalid,
        /// a NotFound result if the user doesn't exist,
        /// an Unauthorized result if the current password is incorrect,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordRequest changeRequest)
        {
            try
            {
                // Step 1: Send password change request to the API endpoint
                HttpResponseMessage response = await _httpClient.PatchAsJsonAsync($"api/users/{changeRequest.UserId}/password", changeRequest);

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Return success result
                    return Result<bool>.Ok(true);
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<bool>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<bool>.NotFound(errorMessage); // e.g., User ID not found
                    case HttpStatusCode.Conflict:
                        return Result<bool>.Conflict(errorMessage); // e.g., Concurrent update issue
                    case HttpStatusCode.Unauthorized:
                        return Result<bool>.Unauthorized(errorMessage);
                    default:
                        return Result<bool>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<bool>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON parsing exceptions
                return Result<bool>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                // Step 7: Handle request timeout exceptions
                return Result<bool>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<bool>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}
