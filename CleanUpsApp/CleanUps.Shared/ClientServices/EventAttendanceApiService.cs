using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Client service for interacting with the Event Attendance API.
    /// Provides methods for managing the relationship between users and the events they attend.
    /// </summary>
    public class EventAttendanceApiService
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
        /// Retrieves all event attendances from the API.
        /// </summary>
        /// <returns>
        /// A Result containing a list of all event attendances if successful,
        /// a NoContent result if no attendances exist,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<List<EventAttendanceResponse>>> GetAllAttendancesAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/eventattendances");
                if (response.IsSuccessStatusCode)
                {
                    List<EventAttendanceResponse>? eventAttendances = await response.Content.ReadFromJsonAsync<List<EventAttendanceResponse>>();
                    return eventAttendances != null ? Result<List<EventAttendanceResponse>>.Ok(eventAttendances) : Result<List<EventAttendanceResponse>>.InternalServerError("Failed to deserialize event attendances");
                }

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
                return Result<List<EventAttendanceResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<List<EventAttendanceResponse>>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<List<EventAttendanceResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all events attended by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose events to retrieve.</param>
        /// <returns>
        /// A Result containing a list of events the user is attending if successful,
        /// a NotFound result if the user doesn't exist,
        /// a BadRequest result if the userId is invalid,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/eventattendances/user/{userId}/events");
                if (response.IsSuccessStatusCode)
                {
                    List<EventResponse>? eventAttendance = await response.Content.ReadFromJsonAsync<List<EventResponse>>();
                    return eventAttendance != null ? Result<List<EventResponse>>.Ok(eventAttendance) : Result<List<EventResponse>>.InternalServerError("Failed to deserialize event");
                }

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
                return Result<List<EventResponse>>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<List<EventResponse>>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<List<EventResponse>>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all users attending a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event whose attendees to retrieve.</param>
        /// <returns>
        /// A Result containing a list of users attending the event if successful,
        /// a NotFound result if the event doesn't exist,
        /// a BadRequest result if the eventId is invalid,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/eventattendances/event/{eventId}/users");
                if (response.IsSuccessStatusCode)
                {
                    List<UserResponse>? eventAttendance = await response.Content.ReadFromJsonAsync<List<UserResponse>>();
                    return eventAttendance != null ? Result<List<UserResponse>>.Ok(eventAttendance) : Result<List<UserResponse>>.InternalServerError("Failed to deserialize user");
                }

                string errorMessage = await response.Content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return Result<List<UserResponse>>.BadRequest(errorMessage);
                    case HttpStatusCode.NotFound:
                        return Result<List<UserResponse>>.NotFound(errorMessage);
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
        /// Creates a new event attendance record through the API.
        /// </summary>
        /// <param name="newEventAttendance">The data for creating a new event attendance.</param>
        /// <returns>
        /// A Result containing the created event attendance if successful,
        /// a BadRequest result if the request data is invalid,
        /// a NotFound result if the user or event doesn't exist,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<EventAttendanceResponse>> CreateAttendanceAsync(CreateEventAttendanceRequest newEventAttendance)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/eventattendances", newEventAttendance);
                if (response.IsSuccessStatusCode)
                {
                    EventAttendanceResponse? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceResponse>();
                    return eventAttendance != null ? Result<EventAttendanceResponse>.Created(eventAttendance) : Result<EventAttendanceResponse>.InternalServerError("Failed to deserialize event attendance");
                }

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
                return Result<EventAttendanceResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<EventAttendanceResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<EventAttendanceResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<EventAttendanceResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing event attendance record through the API.
        /// </summary>
        /// <param name="attendanceToUpdate">The updated event attendance data.</param>
        /// <returns>
        /// A Result containing the updated event attendance if successful,
        /// a BadRequest result if the request data is invalid,
        /// a NotFound result if the attendance record doesn't exist,
        /// a Conflict result if there's a concurrency issue,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<EventAttendanceResponse>> UpdateAttendanceAsync(UpdateEventAttendanceRequest attendanceToUpdate)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/eventattendances/user/{attendanceToUpdate.UserId}/event/{attendanceToUpdate.EventId}", attendanceToUpdate);
                if (response.IsSuccessStatusCode)
                {
                    EventAttendanceResponse? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceResponse>();
                    return eventAttendance != null ? Result<EventAttendanceResponse>.Ok(eventAttendance) : Result<EventAttendanceResponse>.InternalServerError("Failed to deserialize event attendance");
                }

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
                return Result<EventAttendanceResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return Result<EventAttendanceResponse>.BadRequest($"Invalid JSON format: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<EventAttendanceResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<EventAttendanceResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an event attendance record through the API.
        /// </summary>
        /// <param name="userId">The ID of the user whose attendance record to delete.</param>
        /// <param name="eventId">The ID of the event from which to remove the attendance.</param>
        /// <returns>
        /// A Result containing the deleted event attendance if successful,
        /// a BadRequest result if the request parameters are invalid,
        /// a NotFound result if the attendance record doesn't exist,
        /// a Conflict result if the attendance record cannot be deleted,
        /// or an error message if the operation fails.
        /// </returns>
        public async Task<Result<EventAttendanceResponse>> DeleteAttendanceAsync(int userId, int eventId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/eventattendances/user/{userId}/event/{eventId}");
                if (response.IsSuccessStatusCode)
                {
                    EventAttendanceResponse? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceResponse>();
                    return eventAttendance != null ? Result<EventAttendanceResponse>.Ok(eventAttendance) : Result<EventAttendanceResponse>.InternalServerError("Failed to deserialize event attendance");
                }

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
                return Result<EventAttendanceResponse>.InternalServerError($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                return Result<EventAttendanceResponse>.InternalServerError("Request timed out");
            }
            catch (Exception ex)
            {
                return Result<EventAttendanceResponse>.InternalServerError($"Unexpected error: {ex.Message}");
            }
        }
    }
}