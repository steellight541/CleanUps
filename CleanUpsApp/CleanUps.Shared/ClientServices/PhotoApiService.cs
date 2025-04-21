using CleanUps.Shared.ClientServices.Interfaces;
using CleanUps.Shared.DTOs.Photos;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Photos API endpoints.
    /// Provides methods for performing CRUD operations on photos through HTTP requests.
    /// Implements standardized error handling using the Result pattern for consistent response handling.
    /// </summary>
    public class PhotoApiService : IPhotoApiService
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
        /// Retrieves all photos from the API endpoint.
        /// </summary>
        /// <returns>
        /// A Result containing a list of all photos if successful,
        /// a NoContent result if no photos exist in the system,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<List<PhotoResponse>>> GetAllAsync()
        {
            try
            {
                // Step 1: Send request to the API endpoint to retrieve all photos
                HttpResponseMessage response = await _httpClient.GetAsync("api/photos");
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the successful response
                    List<PhotoResponse>? photos = await response.Content.ReadFromJsonAsync<List<PhotoResponse>>();
                    return photos != null 
                        ? Result<List<PhotoResponse>>.Ok(photos) 
                        : Result<List<PhotoResponse>>.InternalServerError("Failed to deserialize photos");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<List<PhotoResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<List<PhotoResponse>>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<List<PhotoResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific photo by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the photo to retrieve.</param>
        /// <returns>
        /// A Result containing the requested photo if found,
        /// a NotFound result if the photo doesn't exist,
        /// a BadRequest result if the ID format is invalid,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<PhotoResponse>> GetByIdAsync(int id)
        {
            try
            {
                // Step 1: Send request to retrieve a specific photo by its ID
                HttpResponseMessage response = await _httpClient.GetAsync($"api/photos/{id}");
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the photo from the response
                    PhotoResponse? photo = await response.Content.ReadFromJsonAsync<PhotoResponse>();
                    return photo != null 
                        ? Result<PhotoResponse>.Ok(photo) 
                        : Result<PhotoResponse>.InternalServerError("Failed to deserialize photo");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<PhotoResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<PhotoResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<PhotoResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all photos associated with a specific event.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event whose photos to retrieve.</param>
        /// <returns>
        /// A Result containing a list of photos if successful,
        /// a NotFound result if the event doesn't exist,
        /// a BadRequest result if the event ID format is invalid,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<List<PhotoResponse>>> GetPhotosByEventIdAsync(int eventId)
        {
            try
            {
                // Step 1: Send request to retrieve photos for the specified event
                HttpResponseMessage response = await _httpClient.GetAsync($"api/photos/events/{eventId}");
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the photos list from the response
                    List<PhotoResponse>? photos = await response.Content.ReadFromJsonAsync<List<PhotoResponse>>();
                    return photos != null 
                        ? Result<List<PhotoResponse>>.Ok(photos) 
                        : Result<List<PhotoResponse>>.InternalServerError("Failed to deserialize photos");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<List<PhotoResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<List<PhotoResponse>>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<List<PhotoResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new photo through the API endpoint.
        /// </summary>
        /// <param name="createRequest">The data transfer object containing image data, caption, and event information.</param>
        /// <returns>
        /// A Result containing the created photo if successful,
        /// a BadRequest result if the request data is invalid or incomplete,
        /// a NotFound result if the referenced event doesn't exist,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<PhotoResponse>> CreateAsync(CreatePhotoRequest createRequest)
        {
            try
            {
                // Step 1: Send create request to the API endpoint
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/photos", createRequest);
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the created photo from response
                    PhotoResponse? createdPhoto = await response.Content.ReadFromJsonAsync<PhotoResponse>();
                    return createdPhoto != null 
                        ? Result<PhotoResponse>.Created(createdPhoto) 
                        : Result<PhotoResponse>.InternalServerError("Failed to deserialize photo");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<PhotoResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON formatting exceptions
                return Result<PhotoResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 7: Handle request timeout exceptions
                return Result<PhotoResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<PhotoResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing photo through the API endpoint (typically to change its caption or metadata).
        /// </summary>
        /// <param name="updateRequest">The data transfer object containing the updated photo information.</param>
        /// <returns>
        /// A Result containing the updated photo if successful,
        /// a BadRequest result if the request data is invalid or incomplete,
        /// a NotFound result if the photo doesn't exist,
        /// a Conflict result if there's a concurrency issue or business rule violation,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<PhotoResponse>> UpdateAsync(UpdatePhotoRequest updateRequest)
        {
            try
            {
                // Step 1: Send update request to the API endpoint
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/photos/{updateRequest.PhotoId}", updateRequest);
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the updated photo from response
                    PhotoResponse? updatedPhoto = await response.Content.ReadFromJsonAsync<PhotoResponse>();
                    return updatedPhoto != null 
                        ? Result<PhotoResponse>.Ok(updatedPhoto) 
                        : Result<PhotoResponse>.InternalServerError("Failed to deserialize photo");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<PhotoResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON formatting exceptions
                return Result<PhotoResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 7: Handle request timeout exceptions
                return Result<PhotoResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<PhotoResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a photo through the API endpoint.
        /// </summary>
        /// <param name="deleteRequest">The data transfer object containing the ID of the photo to delete.</param>
        /// <returns>
        /// A Result containing the deleted photo if successful,
        /// a BadRequest result if the ID format is invalid,
        /// a NotFound result if the photo doesn't exist,
        /// a Conflict result if the photo cannot be deleted due to relationships or permissions,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<PhotoResponse>> DeleteAsync(DeletePhotoRequest deleteRequest)
        {
            try
            {
                // Step 1: Send delete request to the API endpoint
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/photos/{deleteRequest.PhotoId}");
                
                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the deleted photo from response
                    PhotoResponse? deletedPhoto = await response.Content.ReadFromJsonAsync<PhotoResponse>();
                    return deletedPhoto != null 
                        ? Result<PhotoResponse>.Ok(deletedPhoto) 
                        : Result<PhotoResponse>.InternalServerError("Failed to deserialize photo");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<PhotoResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<PhotoResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<PhotoResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}