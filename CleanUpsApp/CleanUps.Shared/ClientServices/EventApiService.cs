using CleanUps.Shared.ClientServices.Interfaces;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Events API endpoints.
    /// Provides methods for creating, reading, updating, and deleting event data through HTTP requests.
    /// Implements standardized error handling using the Result pattern for consistent response handling.
    /// </summary>
    public class EventApiService : IEventApiService
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
        /// Retrieves all events from the API endpoint.
        /// </summary>
        /// <returns>
        /// A Result containing a list of all events if successful,
        /// a NoContent result if no events exist in the system,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<List<EventResponse>>> GetAllAsync()
        {
            try
            {
                // Step 1: Send request to the API endpoint to retrieve all events
                HttpResponseMessage response = await _httpClient.GetAsync("api/events");

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the successful response
                    List<EventResponse>? events = await response.Content.ReadFromJsonAsync<List<EventResponse>>();
                    return events != null
                        ? Result<List<EventResponse>>.Ok(events)
                        : Result<List<EventResponse>>.InternalServerError("Failed to deserialize events");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<List<EventResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<List<EventResponse>>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<List<EventResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a specific event by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the event to retrieve.</param>
        /// <returns>
        /// A Result containing the requested event if found,
        /// a NotFound result if the event doesn't exist,
        /// a BadRequest result if the ID format is invalid,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<EventResponse>> GetByIdAsync(int id)
        {
            try
            {
                // Step 1: Send request to the API endpoint to retrieve specific event
                HttpResponseMessage response = await _httpClient.GetAsync($"api/events/{id}");

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the successful response
                    EventResponse? eventDto = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return eventDto != null
                        ? Result<EventResponse>.Ok(eventDto)
                        : Result<EventResponse>.InternalServerError("Failed to deserialize event");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new event by sending event data to the API.
        /// </summary>
        /// <param name="createRequest">The data transfer object containing event details for creation.</param>
        /// <returns>
        /// A Result containing the created event with its generated ID if successful,
        /// a BadRequest result if the event data is invalid or missing required fields,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<EventResponse>> CreateAsync(CreateEventRequest createRequest)
        {
            try
            {
                // Step 1: Send create request to the API endpoint
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/events", createRequest);

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the created event from the response
                    EventResponse? createdEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return createdEvent != null
                        ? Result<EventResponse>.Created(createdEvent)
                        : Result<EventResponse>.InternalServerError("Failed to deserialize event");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON parsing exceptions
                return Result<EventResponse>.BadRequest($"Invalid request format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 7: Handle request timeout exceptions
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing event with new information.
        /// </summary>
        /// <param name="updateRequest">The data transfer object containing updated event details, including its identifier.</param>
        /// <returns>
        /// A Result containing the updated event if successful,
        /// a BadRequest result if the request data is invalid,
        /// a NotFound result if the event doesn't exist,
        /// a Conflict result if there's a concurrency issue,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<EventResponse>> UpdateAsync(UpdateEventRequest updateRequest)
        {
            try
            {
                // Step 1: Send update request to the API endpoint
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/events/{updateRequest.EventId}", updateRequest);

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the updated event from response
                    EventResponse? updatedEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return updatedEvent != null
                        ? Result<EventResponse>.Ok(updatedEvent)
                        : Result<EventResponse>.InternalServerError("Failed to deserialize event");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON parsing exceptions
                return Result<EventResponse>.BadRequest($"Invalid request format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 7: Handle request timeout exceptions
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an event from the system.
        /// </summary>
        /// <param name="deleteRequest">The data transfer object containing the event ID to delete.</param>
        /// <returns>
        /// A Result containing the deleted event data if successful,
        /// a BadRequest result if the ID is invalid,
        /// a NotFound result if the event doesn't exist,
        /// a Conflict result if the event cannot be deleted (e.g., referenced by other entities),
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<EventResponse>> DeleteAsync(DeleteEventRequest deleteRequest)
        {
            try
            {
                // Step 1: Send delete request to the API endpoint
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/events/{deleteRequest.EventId}");

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the deleted event from response
                    EventResponse? deletedEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return deletedEvent != null
                        ? Result<EventResponse>.Ok(deletedEvent)
                        : Result<EventResponse>.InternalServerError("Failed to deserialize event");
                }

                // Step 4: Handle error responses based on status code
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
                // Step 5: Handle network-related exceptions
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the status of an existing event via the API and returns the updated event.
        /// </summary>
        /// <param name="request">The request containing the event ID and the new status.</param>
        /// <returns>
        /// A Result containing the updated <see cref="EventResponse"/> if successful.
        /// Returns BadRequest if the request data is invalid,
        /// NotFound if the event doesn't exist,
        /// Conflict if there's a concurrency issue or the status update is invalid,
        /// or an error message for other network/server issues.
        /// </returns>
        public async Task<Result<EventResponse>> UpdateStatusAsync(UpdateEventStatusRequest request)
        {
            try
            {
                // Step 1: Send PATCH request to the status update endpoint
                HttpResponseMessage response = await _httpClient.PatchAsJsonAsync($"api/events/{request.EventId}/status", request);

                // Step 2: Process successful response (200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the updated event from the response
                    EventResponse? updatedEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                    return updatedEvent != null 
                        ? Result<EventResponse>.Ok(updatedEvent)
                        : Result<EventResponse>.InternalServerError("Failed to deserialize updated event.");
                }

                // Step 3: Handle error responses based on status code
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
                // Step 4: Handle network-related exceptions
                return Result<EventResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 5: Handle JSON parsing exceptions (for request or response)
                return Result<EventResponse>.BadRequest($"Invalid data format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<EventResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<EventResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}