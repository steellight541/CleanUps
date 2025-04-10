using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Users API.
    /// Provides methods for performing CRUD operations on users through HTTP requests.
    /// </summary>
    public class UserApiService
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
        /// Retrieves all users from the API.
        /// </summary>
        /// <returns>
        /// A Result containing a list of all users if successful,
        /// a NoContent result if no users exist,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<List<UserResponse>>> GetAllUsersAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/users");
                if (response.IsSuccessStatusCode)
                {
                    List<UserResponse>? users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
                    return users != null ? Result<List<UserResponse>>.Ok(users) : Result<List<UserResponse>>.InternalServerError("Failed to deserialize users");
                }

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
                return Result<List<UserResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<List<UserResponse>>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
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
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<UserResponse>> GetUserByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    UserResponse? user = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return user != null ? Result<UserResponse>.Ok(user) : Result<UserResponse>.InternalServerError("Failed to deserialize user");
                }

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
                return Result<UserResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<UserResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
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
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest createRequest)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/users", createRequest);
                if (response.IsSuccessStatusCode)
                {
                    UserResponse? createdUser = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return createdUser != null ? Result<UserResponse>.Created(createdUser) : Result<UserResponse>.InternalServerError("Failed to deserialize user");
                }

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
                return Result<UserResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<UserResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<UserResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<UserResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user through the API.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updateRequest">The updated user data.</param>
        /// <returns>
        /// A Result containing the updated user if successful,
        /// a BadRequest result if the request data is invalid,
        /// a NotFound result if the user doesn't exist,
        /// a Conflict result if there's a concurrency issue or email conflict,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest updateRequest)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/users/{id}", updateRequest);
                if (response.IsSuccessStatusCode)
                {
                    UserResponse? updatedUser = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return updatedUser != null ? Result<UserResponse>.Ok(updatedUser) : Result<UserResponse>.InternalServerError("Failed to deserialize user");
                }

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
                return Result<UserResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<UserResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<UserResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<UserResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a user through the API.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>
        /// A Result containing the deleted user if successful,
        /// a BadRequest result if the ID is invalid,
        /// a NotFound result if the user doesn't exist,
        /// a Conflict result if the user cannot be deleted,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<UserResponse>> DeleteUserAsync(int userId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/users/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    UserResponse? deletedUser = await response.Content.ReadFromJsonAsync<UserResponse>();
                    return deletedUser != null ? Result<UserResponse>.Ok(deletedUser) : Result<UserResponse>.InternalServerError("Failed to deserialize user");
                }

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
                return Result<UserResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<UserResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<UserResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}
