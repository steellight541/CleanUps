using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;

namespace CleanUps.Shared.ClientServices
{
    public class UserApiService
    {
        private readonly HttpClient _http;

        public UserApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Result<List<UserResponse>>> GetAllUsersAsync()
        {
            HttpResponseMessage response = await _http.GetAsync("api/users");
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

        public async Task<Result<UserResponse>> GetUserByIdAsync(int id)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/users/{id}");
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

        public async Task<Result<CreateUserRequest>> CreateUserAsync(CreateUserRequest createRequest)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/users", createRequest);
            if (response.IsSuccessStatusCode)
            {
                CreateUserRequest? createdUser = await response.Content.ReadFromJsonAsync<CreateUserRequest>();
                return createdUser != null ? Result<CreateUserRequest>.Created(createdUser) : Result<CreateUserRequest>.InternalServerError("Failed to deserialize user");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<CreateUserRequest>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<CreateUserRequest>.NotFound(errorMessage);
                default:
                    return Result<CreateUserRequest>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<UserResponse>> UpdateUserAsync(int id, UpdateUserRequest updateRequest)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync($"api/users/{id}", updateRequest);
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

        public async Task<Result<UserResponse>> DeleteUserAsync(int userId)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"api/users/{userId}");
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
    }
}
