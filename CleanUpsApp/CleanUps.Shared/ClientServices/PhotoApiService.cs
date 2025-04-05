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

        public async Task<Result<List<PhotoDTO>>> GetAllPhotosAsync()
        {
            HttpResponseMessage response = await _http.GetAsync("api/photos");
            if (response.IsSuccessStatusCode)
            {
                List<PhotoDTO>? photos = await response.Content.ReadFromJsonAsync<List<PhotoDTO>>();
                return photos != null ? Result<List<PhotoDTO>>.Ok(photos) : Result<List<PhotoDTO>>.InternalServerError("Failed to deserialize photos");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return Result<List<PhotoDTO>>.NoContent();
                default:
                    return Result<List<PhotoDTO>>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<PhotoDTO>> GetPhotoByIdAsync(int id)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/photos/{id}");
            if (response.IsSuccessStatusCode)
            {
                PhotoDTO? photo = await response.Content.ReadFromJsonAsync<PhotoDTO>();
                return photo != null ? Result<PhotoDTO>.Ok(photo) : Result<PhotoDTO>.InternalServerError("Failed to deserialize photo");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<PhotoDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<PhotoDTO>.NotFound(errorMessage);
                default:
                    return Result<PhotoDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<List<PhotoDTO>>> GetPhotosByEventIdAsync(int userId)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/photos/events/{userId}");
            if (response.IsSuccessStatusCode)
            {
                List<PhotoDTO>? photos = await response.Content.ReadFromJsonAsync<List<PhotoDTO>>();
                return photos != null ? Result<List<PhotoDTO>>.Ok(photos) : Result<List<PhotoDTO>>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<List<PhotoDTO>>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<List<PhotoDTO>>.NotFound(errorMessage);
                default:
                    return Result<List<PhotoDTO>>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<PhotoDTO>> CreatePhotoAsync(PhotoDTO newPhoto)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/photos", newPhoto);
            if (response.IsSuccessStatusCode)
            {
                PhotoDTO? photo = await response.Content.ReadFromJsonAsync<PhotoDTO>();
                return photo != null ? Result<PhotoDTO>.Created(photo) : Result<PhotoDTO>.InternalServerError("Failed to deserialize photo");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<PhotoDTO>.BadRequest(errorMessage);
                default:
                    return Result<PhotoDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<PhotoDTO>> UpdatePhotoAsync(int id, PhotoDTO photoToUpdate)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync($"api/photos/{id}", photoToUpdate);
            if (response.IsSuccessStatusCode)
            {
                PhotoDTO? photo = await response.Content.ReadFromJsonAsync<PhotoDTO>();
                return photo != null ? Result<PhotoDTO>.Ok(photo) : Result<PhotoDTO>.InternalServerError("Failed to deserialize photo");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<PhotoDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<PhotoDTO>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<PhotoDTO>.Conflict(errorMessage);
                default:
                    return Result<PhotoDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<PhotoDTO>> DeletePhotoAsync(int id)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"api/photos/{id}");
            if (response.IsSuccessStatusCode)
            {
                PhotoDTO? photo = await response.Content.ReadFromJsonAsync<PhotoDTO>();
                return photo != null ? Result<PhotoDTO>.Ok(photo) : Result<PhotoDTO>.InternalServerError("Failed to deserialize photo");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<PhotoDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<PhotoDTO>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<PhotoDTO>.Conflict(errorMessage);
                default:
                    return Result<PhotoDTO>.InternalServerError(errorMessage);
            }
        }
    }
}