using CleanUps.Shared.DTOs.EventAttendances;
using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.DTOs.Users;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;

namespace CleanUps.Shared.ClientServices
{
    public class EventAttendanceApiService
    {
        private readonly HttpClient _http;

        public EventAttendanceApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Result<List<UpdateEventAttendanceRequest>>> GetAllAttendancesAsync()
        {
            HttpResponseMessage response = await _http.GetAsync("api/eventattendances");
            if (response.IsSuccessStatusCode)
            {
                List<UpdateEventAttendanceRequest>? eventAttendances = await response.Content.ReadFromJsonAsync<List<UpdateEventAttendanceRequest>>();
                return eventAttendances != null ? Result<List<UpdateEventAttendanceRequest>>.Ok(eventAttendances) : Result<List<UpdateEventAttendanceRequest>>.InternalServerError("Failed to deserialize event attendances");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return Result<List<UpdateEventAttendanceRequest>>.NoContent();
                default:
                    return Result<List<UpdateEventAttendanceRequest>>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<List<EventResponse>>> GetEventsByUserIdAsync(int userId)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/eventattendances/user/{userId}/events");
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
            HttpResponseMessage response = await _http.GetAsync($"api/eventattendances/event/{eventId}/users");
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

        public async Task<Result<UpdateEventAttendanceRequest>> CreateAttendanceAsync(CreateEventAttendanceRequest newEventAttendance)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/eventattendances", newEventAttendance);
            if (response.IsSuccessStatusCode)
            {
                UpdateEventAttendanceRequest? eventAttendance = await response.Content.ReadFromJsonAsync<UpdateEventAttendanceRequest>();
                return eventAttendance != null ? Result<UpdateEventAttendanceRequest>.Created(eventAttendance) : Result<UpdateEventAttendanceRequest>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<UpdateEventAttendanceRequest>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<UpdateEventAttendanceRequest>.NotFound(errorMessage);
                default:
                    return Result<UpdateEventAttendanceRequest>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<UpdateEventAttendanceRequest>> UpdateAttendanceAsync(UpdateEventAttendanceRequest attendanceToUpdate)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync($"api/eventattendances/user/{attendanceToUpdate.UserId}/event/{attendanceToUpdate.EventId}", attendanceToUpdate);
            if (response.IsSuccessStatusCode)
            {
                UpdateEventAttendanceRequest? eventAttendance = await response.Content.ReadFromJsonAsync<UpdateEventAttendanceRequest>();
                return eventAttendance != null ? Result<UpdateEventAttendanceRequest>.Ok(eventAttendance) : Result<UpdateEventAttendanceRequest>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<UpdateEventAttendanceRequest>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<UpdateEventAttendanceRequest>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<UpdateEventAttendanceRequest>.Conflict(errorMessage);
                default:
                    return Result<UpdateEventAttendanceRequest>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<UpdateEventAttendanceRequest>> DeleteAttendanceAsync(int userId, int eventId)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"api/eventattendances/user/{userId}/event/{eventId}");
            if (response.IsSuccessStatusCode)
            {
                UpdateEventAttendanceRequest? eventAttendance = await response.Content.ReadFromJsonAsync<UpdateEventAttendanceRequest>();
                return eventAttendance != null ? Result<UpdateEventAttendanceRequest>.Ok(eventAttendance) : Result<UpdateEventAttendanceRequest>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<UpdateEventAttendanceRequest>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<UpdateEventAttendanceRequest>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<UpdateEventAttendanceRequest>.Conflict(errorMessage);
                default:
                    return Result<UpdateEventAttendanceRequest>.InternalServerError(errorMessage);
            }
        }
    }
}