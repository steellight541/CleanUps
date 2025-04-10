using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Events API.
    /// Provides methods for performing CRUD operations on events through HTTP requests.
    /// Implements error handling with Result pattern instead of throwing exceptions.
    /// </summary>
    public class EventApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventApiService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The factory for creating HTTP clients with preconfigured settings.</param>
        public EventApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CleanupsApi");
        }

        /// <summary>
        /// Retrieves all events from the API.
        /// </summary>
        /// <returns>
        /// A Result containing a list of all events if successful,
        /// a NoContent result if no events exist,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<List<EventResponse>>> GetAllEventsAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/events");
                
                if (response.IsSuccessStatusCode)
                {
                    List<EventResponse>? events = await response.Content.ReadFromJsonAsync<List<EventResponse>>();
                    return events != null 
                        ? Result<List<EventResponse>>.Ok(events) 
                        : Result<List<EventResponse>>.InternalServerError("Failed to deserialize events");
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        return Result<List<EventResponse>>.NoContent();
                    default:
                        return Result<List<EventResponse>>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<List<EventResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return Result<List<EventResponse>>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<List<EventResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        /// <returns>
        /// A Result containing the requested event if found,
        /// a NotFound result if the event doesn't exist,
        /// a BadRequest result if the ID is invalid,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<EventResponse>> GetEventByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/events/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    EventResponse? eventDto = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return eventDto != null 
                        ? Result<EventResponse>.Ok(eventDto) 
                        : Result<EventResponse>.InternalServerError("Failed to deserialize event");
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<EventResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<EventResponse>.NotFound(errorMessage);
                    default:
                        return Result<EventResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new event through the API.
        /// </summary>
        /// <param name="createRequest">The data for creating a new event.</param>
        /// <returns>
        /// A Result containing the created event if successful,
        /// a BadRequest result if the request data is invalid,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<EventResponse>> CreateEventAsync(CreateEventRequest createRequest)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/events", createRequest);
                
                if (response.IsSuccessStatusCode)
                {
                    EventResponse? createdEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return createdEvent != null 
                        ? Result<EventResponse>.Created(createdEvent) 
                        : Result<EventResponse>.InternalServerError("Failed to deserialize event");
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<EventResponse>.BadRequest(errorMessage);
                    default:
                        return Result<EventResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<EventResponse>.BadRequest($"Invalid request format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing event through the API.
        /// </summary>
        /// <param name="id">The ID of the event to update.</param>
        /// <param name="updateRequest">The updated event data.</param>
        /// <returns>
        /// A Result containing the updated event if successful,
        /// a BadRequest result if the request data is invalid,
        /// a NotFound result if the event doesn't exist,
        /// a Conflict result if there's a concurrency issue,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<EventResponse>> UpdateEventAsync(int id, UpdateEventRequest updateRequest)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/events/{id}", updateRequest);
                
                if (response.IsSuccessStatusCode)
                {
                    EventResponse? updatedEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return updatedEvent != null 
                        ? Result<EventResponse>.Ok(updatedEvent) 
                        : Result<EventResponse>.InternalServerError("Failed to deserialize event");
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<EventResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<EventResponse>.NotFound(errorMessage);
                    case HttpStatusCode.Conflict:
                        return Result<EventResponse>.Conflict(errorMessage);
                    default:
                        return Result<EventResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<EventResponse>.BadRequest($"Invalid request format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an event through the API.
        /// </summary>
        /// <param name="eventId">The ID of the event to delete.</param>
        /// <returns>
        /// A Result containing the deleted event if successful,
        /// a BadRequest result if the ID is invalid,
        /// a NotFound result if the event doesn't exist,
        /// a Conflict result if the event cannot be deleted,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<EventResponse>> DeleteEventAsync(int eventId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/events/{eventId}");
                
                if (response.IsSuccessStatusCode)
                {
                    EventResponse? deletedEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return deletedEvent != null 
                        ? Result<EventResponse>.Ok(deletedEvent) 
                        : Result<EventResponse>.InternalServerError("Failed to deserialize event");
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<EventResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<EventResponse>.NotFound(errorMessage);
                    case HttpStatusCode.Conflict:
                        return Result<EventResponse>.Conflict(errorMessage);
                    default:
                        return Result<EventResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}