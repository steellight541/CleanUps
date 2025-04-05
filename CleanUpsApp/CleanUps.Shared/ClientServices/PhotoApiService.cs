using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;

namespace CleanUps.Shared.ClientServices
{
    public class PhotoApiService
    {
        private readonly HttpClient _http;

        public PhotoApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Result<List<PhotoResponse>>> GetAllPhotosAsync()
        {
            HttpResponseMessage response = await _http.GetAsync("api/photos");
            if (response.IsSuccessStatusCode)
            {
                List<PhotoResponse>? photos = await response.Content.ReadFromJsonAsync<List<PhotoResponse>>();
                return photos != null ? Result<List<PhotoResponse>>.Ok(photos) : Result<List<PhotoResponse>>.InternalServerError("Failed to deserialize photos");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return Result<List<PhotoResponse>>.NoContent();
                default:
                    return Result<List<PhotoResponse>>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<PhotoResponse>> GetPhotoByIdAsync(int id)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/photos/{id}");
            if (response.IsSuccessStatusCode)
            {
                PhotoResponse? photo = await response.Content.ReadFromJsonAsync<PhotoResponse>();
                return photo != null ? Result<PhotoResponse>.Ok(photo) : Result<PhotoResponse>.InternalServerError("Failed to deserialize photo");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<PhotoResponse>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<PhotoResponse>.NotFound(errorMessage);
                default:
                    return Result<PhotoResponse>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<List<PhotoResponse>>> GetPhotosByEventIdAsync(int eventId)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/photos/events/{eventId}");
            if (response.IsSuccessStatusCode)
            {
                List<PhotoResponse>? photos = await response.Content.ReadFromJsonAsync<List<PhotoResponse>>();
                return photos != null ? Result<List<PhotoResponse>>.Ok(photos) : Result<List<PhotoResponse>>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<List<PhotoResponse>>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<List<PhotoResponse>>.NotFound(errorMessage);
                default:
                    return Result<List<PhotoResponse>>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<PhotoResponse>> CreatePhotoAsync(CreatePhotoRequest createRequest)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/photos", createRequest);
            if (response.IsSuccessStatusCode)
            {
                PhotoResponse? createdPhoto = await response.Content.ReadFromJsonAsync<PhotoResponse>();
                return createdPhoto != null ? Result<PhotoResponse>.Created(createdPhoto) : Result<PhotoResponse>.InternalServerError("Failed to deserialize photo");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<PhotoResponse>.BadRequest(errorMessage);
                default:
                    return Result<PhotoResponse>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<PhotoResponse>> UpdatePhotoAsync(int id, UpdatePhotoRequest updateRequest)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync($"api/photos/{id}", updateRequest);
            if (response.IsSuccessStatusCode)
            {
                PhotoResponse? updatedPhoto = await response.Content.ReadFromJsonAsync<PhotoResponse>();
                return updatedPhoto != null ? Result<PhotoResponse>.Ok(updatedPhoto) : Result<PhotoResponse>.InternalServerError("Failed to deserialize photo");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<PhotoResponse>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<PhotoResponse>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<PhotoResponse>.Conflict(errorMessage);
                default:
                    return Result<PhotoResponse>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<PhotoResponse>> DeletePhotoAsync(int id)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"api/photos/{id}");
            if (response.IsSuccessStatusCode)
            {
                PhotoResponse? deletedPhoto = await response.Content.ReadFromJsonAsync<PhotoResponse>();
                return deletedPhoto != null ? Result<PhotoResponse>.Ok(deletedPhoto) : Result<PhotoResponse>.InternalServerError("Failed to deserialize photo");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<PhotoResponse>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<PhotoResponse>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<PhotoResponse>.Conflict(errorMessage);
                default:
                    return Result<PhotoResponse>.InternalServerError(errorMessage);
            }
        }
    }
}