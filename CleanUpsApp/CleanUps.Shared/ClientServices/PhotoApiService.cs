using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Photos API.
    /// Provides methods for performing CRUD operations on photos through HTTP requests.
    /// </summary>
    public class PhotoApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoApiService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The factory for creating HTTP clients with preconfigured settings.</param>
        public PhotoApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CleanupsApi");
        }

        /// <summary>
        /// Retrieves all photos from the API.
        /// </summary>
        /// <returns>
        /// A Result containing a list of all photos if successful,
        /// a NoContent result if no photos exist,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<List<PhotoResponse>>> GetAllPhotosAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/photos");
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
            catch (HttpRequestException ex)
            {
                return Result<List<PhotoResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<List<PhotoResponse>>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<List<PhotoResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific photo by its ID.
        /// </summary>
        /// <param name="id">The ID of the photo to retrieve.</param>
        /// <returns>
        /// A Result containing the requested photo if found,
        /// a NotFound result if the photo doesn't exist,
        /// a BadRequest result if the ID is invalid,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<PhotoResponse>> GetPhotoByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/photos/{id}");
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
            catch (HttpRequestException ex)
            {
                return Result<PhotoResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<PhotoResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<PhotoResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all photos associated with a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event whose photos to retrieve.</param>
        /// <returns>
        /// A Result containing a list of photos if successful,
        /// a NotFound result if the event doesn't exist,
        /// a BadRequest result if the event ID is invalid,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<List<PhotoResponse>>> GetPhotosByEventIdAsync(int eventId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/photos/events/{eventId}");
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
            catch (HttpRequestException ex)
            {
                return Result<List<PhotoResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<List<PhotoResponse>>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<List<PhotoResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new photo through the API.
        /// </summary>
        /// <param name="createRequest">The data for creating a new photo, including image data and event information.</param>
        /// <returns>
        /// A Result containing the created photo if successful,
        /// a BadRequest result if the request data is invalid,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<PhotoResponse>> CreatePhotoAsync(CreatePhotoRequest createRequest)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/photos", createRequest);
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
            catch (HttpRequestException ex)
            {
                return Result<PhotoResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<PhotoResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<PhotoResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<PhotoResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing photo through the API (typically to change its caption).
        /// </summary>
        /// <param name="id">The ID of the photo to update.</param>
        /// <param name="updateRequest">The updated photo data.</param>
        /// <returns>
        /// A Result containing the updated photo if successful,
        /// a BadRequest result if the request data is invalid,
        /// a NotFound result if the photo doesn't exist,
        /// a Conflict result if there's a concurrency issue,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<PhotoResponse>> UpdatePhotoAsync(int id, UpdatePhotoRequest updateRequest)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/photos/{id}", updateRequest);
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
            catch (HttpRequestException ex)
            {
                return Result<PhotoResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<PhotoResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<PhotoResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<PhotoResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a photo through the API.
        /// </summary>
        /// <param name="photoId">The ID of the photo to delete.</param>
        /// <returns>
        /// A Result containing the deleted photo if successful,
        /// a BadRequest result if the ID is invalid,
        /// a NotFound result if the photo doesn't exist,
        /// a Conflict result if the photo cannot be deleted,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<PhotoResponse>> DeletePhotoAsync(int photoId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/photos/{photoId}");
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
            catch (HttpRequestException ex)
            {
                return Result<PhotoResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<PhotoResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<PhotoResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}