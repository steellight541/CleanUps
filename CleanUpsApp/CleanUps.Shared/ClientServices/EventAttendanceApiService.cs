using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace CleanUps.Shared.ClientServices
{
    public class EventAttendanceApiService
    {
        private readonly HttpClient _httpClient;

        public EventAttendanceApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CleanupsApi");
        }

        public async Task<Result<List<EventAttendanceResponse>>> GetAllAttendancesAsync()
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

        public async Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId)
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
        public async Task<Result<List<UserResponse>>> GetUsersByEventIdAsync(int eventId)
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

        public async Task<Result<EventAttendanceResponse>> CreateAttendanceAsync(CreateEventAttendanceRequest newEventAttendance)
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

        public async Task<Result<EventAttendanceResponse>> UpdateAttendanceAsync(UpdateEventAttendanceRequest attendanceToUpdate)
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

        public async Task<Result<EventAttendanceResponse>> DeleteAttendanceAsync(int userId, int eventId)
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
    }
}