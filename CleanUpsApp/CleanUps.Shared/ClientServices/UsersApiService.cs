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

        public async Task<Result<List<UserDTO>>> GetAllUsersAsync()
        {
            HttpResponseMessage response = await _http.GetAsync("api/users");
            if (response.IsSuccessStatusCode)
            {
                List<UserDTO>? users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                return users != null ? Result<List<UserDTO>>.Ok(users) : Result<List<UserDTO>>.InternalServerError("Failed to deserialize users");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<List<UserDTO>>.BadRequest(errorMessage);
                case HttpStatusCode.NoContent:
                    return Result<List<UserDTO>>.NoContent();
                default:
                    return Result<List<UserDTO>>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<UserDTO>> GetUserByIdAsync(int id)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
                return user != null ? Result<UserDTO>.Ok(user) : Result<UserDTO>.InternalServerError("Failed to deserialize user");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<UserDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<UserDTO>.NotFound(errorMessage);
                default:
                    return Result<UserDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<CreateUserRequest>> CreateAccountAsync(CreateUserRequest newUser)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/users", newUser);
            if (response.IsSuccessStatusCode)
            {
                CreateUserRequest? user = await response.Content.ReadFromJsonAsync<CreateUserRequest>();
                return user != null ? Result<CreateUserRequest>.Created(user) : Result<CreateUserRequest>.InternalServerError("Failed to deserialize user");
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

        public async Task<Result<UserDTO>> UpdateUserAsync(int id, UserDTO userToUpdate)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync($"api/users/{id}", userToUpdate);
            if (response.IsSuccessStatusCode)
            {
                UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
                return user != null ? Result<UserDTO>.Ok(user) : Result<UserDTO>.InternalServerError("Failed to deserialize user");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<UserDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<UserDTO>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<UserDTO>.Conflict(errorMessage);
                default:
                    return Result<UserDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<UserDTO>> DeleteUserAsync(int id)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"api/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();
                return user != null ? Result<UserDTO>.Ok(user) : Result<UserDTO>.InternalServerError("Failed to deserialize user");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<UserDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<UserDTO>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<UserDTO>.Conflict(errorMessage);
                default:
                    return Result<UserDTO>.InternalServerError(errorMessage);
            }
        }
    }
}
