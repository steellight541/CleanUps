using CleanUps.Shared.ClientServices.Interfaces;
using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Event Attendance API endpoints.
    /// Provides methods for managing the relationship between users and the events they attend,
    /// including retrieving, creating, updating, and deleting attendance records.
    /// Implements standardized error handling using the Result pattern for consistent response handling.
    /// </summary>
    public class EventAttendanceApiService : IEventAttendanceApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAttendanceApiService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The factory for creating HTTP clients with preconfigured settings.</param>
        public EventAttendanceApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CleanupsApi");
        }

        /// <summary>
        /// Retrieves all event attendances from the API endpoint.
        /// </summary>
        /// <returns>
        /// A Result containing a list of all event attendances if successful,
        /// a NoContent result if no attendances exist in the system,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<List<EventAttendanceResponse>>> GetAllAsync()
        {
            try
            {
                // Step 1: Send request to the API endpoint to retrieve all event attendances
                HttpResponseMessage response = await _httpClient.GetAsync("api/eventattendances");

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the successful response
                    List<EventAttendanceResponse>? eventAttendances = await response.Content.ReadFromJsonAsync<List<EventAttendanceResponse>>();
                    return eventAttendances != null 
                        ? Result<List<EventAttendanceResponse>>.Ok(eventAttendances) 
                        : Result<List<EventAttendanceResponse>>.InternalServerError("Failed to deserialize event attendances");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        return Result<List<EventAttendanceResponse>>.NoContent();
                    default:
                        return Result<List<EventAttendanceResponse>>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<List<EventAttendanceResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<List<EventAttendanceResponse>>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<List<EventAttendanceResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all events attended by a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose events to retrieve.</param>
        /// <returns>
        /// A Result containing a list of events the user is attending if successful,
        /// a NotFound result if the user doesn't exist,
        /// a BadRequest result if the userId format is invalid,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId)
        {
            try
            {
                // Step 1: Send request to retrieve events for specific user
                HttpResponseMessage response = await _httpClient.GetAsync($"api/eventattendances/user/{userId}/events");

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize events from the response
                    List<EventResponse>? events = await response.Content.ReadFromJsonAsync<List<EventResponse>>();
                    return events != null 
                        ? Result<List<EventResponse>>.Ok(events) 
                        : Result<List<EventResponse>>.InternalServerError("Failed to deserialize events");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<List<EventResponse>>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<List<EventResponse>>.NotFound(errorMessage);
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
        /// Retrieves all users attending a specific event.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event whose attendees to retrieve.</param>
        /// <returns>
        /// A Result containing a list of users attending the event if successful,
        /// a NotFound result if the event doesn't exist,
        /// a BadRequest result if the eventId format is invalid,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId)
        {
            try
            {
                // Step 1: Send request to retrieve users for specific event
                HttpResponseMessage response = await _httpClient.GetAsync($"api/eventattendances/event/{eventId}/users");
                
                // Step 2: Handle No Content response (no attendees)
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // API indicated no attendees, return success with an empty list
                    return Result<List<UserResponse>>.Ok(new List<UserResponse>()); 
                }

                // Step 3: Process successful response with content
                if (response.IsSuccessStatusCode) // Handles 200 OK
                {
                    // Step 4: Read and deserialize the response content
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    List<UserResponse>? users = JsonSerializer.Deserialize<List<UserResponse>>(
                        jsonContent, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                    
                    // Step 5: Return users list or empty list if deserialization fails
                    return Result<List<UserResponse>>.Ok(users ?? new List<UserResponse>());
                }

                // Step 6: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<List<UserResponse>>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<List<UserResponse>>.NotFound(errorMessage);
                    default:
                         return Result<List<UserResponse>>.InternalServerError($"API Error: {response.StatusCode} - {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 7: Handle network-related exceptions
                return Result<List<UserResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 8: Handle JSON deserialization exceptions
                return Result<List<UserResponse>>.InternalServerError($"Failed to deserialize response: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 9: Handle request timeout exceptions
                return Result<List<UserResponse>>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 10: Handle any other unexpected exceptions
                return Result<List<UserResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new event attendance record through the API.
        /// </summary>
        /// <param name="newEventAttendance">The data transfer object containing the user ID, event ID, and optional check-in time.</param>
        /// <returns>
        /// A Result containing the created event attendance if successful,
        /// a BadRequest result if the request data is invalid or incomplete,
        /// a NotFound result if the referenced user or event doesn't exist,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<EventAttendanceResponse>> CreateAsync(CreateEventAttendanceRequest newEventAttendance)
        {
            try
            {
                // Step 1: Send create request to the API endpoint
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/eventattendances", newEventAttendance);

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the created event attendance from response
                    EventAttendanceResponse? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceResponse>();
                    return eventAttendance != null 
                        ? Result<EventAttendanceResponse>.Created(eventAttendance) 
                        : Result<EventAttendanceResponse>.InternalServerError("Failed to deserialize event attendance");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<EventAttendanceResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<EventAttendanceResponse>.NotFound(errorMessage);
                    default:
                        return Result<EventAttendanceResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON formatting exceptions
                return Result<EventAttendanceResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 7: Handle request timeout exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing event attendance record through the API.
        /// </summary>
        /// <param name="attendanceToUpdate">The data transfer object containing updated attendance information, including user ID, event ID, and check-in time.</param>
        /// <returns>
        /// A Result containing the updated event attendance if successful,
        /// a BadRequest result if the request data is invalid or incomplete,
        /// a NotFound result if the attendance record doesn't exist,
        /// a Conflict result if there's a concurrency issue or business rule violation,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<EventAttendanceResponse>> UpdateAsync(UpdateEventAttendanceRequest attendanceToUpdate)
        {
            try
            {
                // Step 1: Send update request to the API endpoint
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
                    $"api/eventattendances/user/{attendanceToUpdate.UserId}/event/{attendanceToUpdate.EventId}", 
                    attendanceToUpdate
                );

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the updated event attendance from response
                    EventAttendanceResponse? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceResponse>();
                    return eventAttendance != null 
                        ? Result<EventAttendanceResponse>.Ok(eventAttendance) 
                        : Result<EventAttendanceResponse>.InternalServerError("Failed to deserialize event attendance");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<EventAttendanceResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<EventAttendanceResponse>.NotFound(errorMessage);
                    case HttpStatusCode.Conflict:
                        return Result<EventAttendanceResponse>.Conflict(errorMessage);
                    default:
                        return Result<EventAttendanceResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Step 6: Handle JSON formatting exceptions
                return Result<EventAttendanceResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 7: Handle request timeout exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 8: Handle any other unexpected exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an event attendance record through the API.
        /// </summary>
        /// <param name="deleteRequest">The data transfer object containing the user ID and event ID identifying the attendance record to delete.</param>
        /// <returns>
        /// A Result containing the deleted event attendance if successful,
        /// a BadRequest result if the request parameters are invalid,
        /// a NotFound result if the attendance record doesn't exist,
        /// a Conflict result if the attendance record cannot be deleted due to business rules,
        /// or an error message if a network or server error occurs.
        /// </returns>
        public async Task<Result<EventAttendanceResponse>> DeleteAsync(DeleteEventAttendanceRequest deleteRequest)
        {
            try
            {
                // Step 1: Send delete request to the API endpoint
                HttpResponseMessage response = await _httpClient.DeleteAsync(
                    $"api/eventattendances/user/{deleteRequest.UserId}/event/{deleteRequest.EventId}"
                );

                // Step 2: Process successful response
                if (response.IsSuccessStatusCode)
                {
                    // Step 3: Deserialize the deleted event attendance from response
                    EventAttendanceResponse? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceResponse>();
                    return eventAttendance != null 
                        ? Result<EventAttendanceResponse>.Ok(eventAttendance) 
                        : Result<EventAttendanceResponse>.InternalServerError("Failed to deserialize event attendance");
                }

                // Step 4: Handle error responses based on status code
                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<EventAttendanceResponse>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<EventAttendanceResponse>.NotFound(errorMessage);
                    case HttpStatusCode.Conflict:
                        return Result<EventAttendanceResponse>.Conflict(errorMessage);
                    default:
                        return Result<EventAttendanceResponse>.InternalServerError(errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                // Step 5: Handle network-related exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // Step 6: Handle request timeout exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Request timeout: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Step 7: Handle any other unexpected exceptions
                return Result<EventAttendanceResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// This method is intentionally left unimplemented as EventAttendance uses a composite key (UserId, EventId).
        /// Use the appropriate methods specific to EventAttendance's key structure instead.
        /// </summary>
        /// <param name="id">ID parameter (not applicable for EventAttendance).</param>
        /// <returns>
        /// A BadRequest result with a message indicating that this method is not implemented.
        /// </returns>
        public async Task<Result<EventAttendanceResponse>> GetByIdAsync(int id)
        {
            // Step 1: Return bad request as this method is not applicable for composite-keyed entities
            return Result<EventAttendanceResponse>.BadRequest("GetByIdAsync is not implemented in this API");
        }
    }
}