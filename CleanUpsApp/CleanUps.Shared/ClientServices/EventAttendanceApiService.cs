using CleanUps.Shared.DTOs;
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

        public async Task<Result<EventAttendanceDTO>> CreateEventAttendanceAsync(EventAttendanceDTO newEventAttendance)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/eventattendances", newEventAttendance);
            if (response.IsSuccessStatusCode)
            {
                EventAttendanceDTO? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceDTO>();
                return eventAttendance != null ? Result<EventAttendanceDTO>.Created(eventAttendance) : Result<EventAttendanceDTO>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<EventAttendanceDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<EventAttendanceDTO>.NotFound(errorMessage);
                default:
                    return Result<EventAttendanceDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<List<EventAttendanceDTO>>> GetEventAttendancesAsync()
        {
            HttpResponseMessage response = await _http.GetAsync("api/eventattendances");
            if (response.IsSuccessStatusCode)
            {
                List<EventAttendanceDTO>? eventAttendances = await response.Content.ReadFromJsonAsync<List<EventAttendanceDTO>>();
                return eventAttendances != null ? Result<List<EventAttendanceDTO>>.Ok(eventAttendances) : Result<List<EventAttendanceDTO>>.InternalServerError("Failed to deserialize event attendances");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return Result<List<EventAttendanceDTO>>.NoContent();
                default:
                    return Result<List<EventAttendanceDTO>>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<List<EventDTO>>> GetEventsForASingleUserAsync(int userId)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/eventattendances/user/{userId}/events");
            if (response.IsSuccessStatusCode)
            {
                List<EventDTO>? eventAttendance = await response.Content.ReadFromJsonAsync<List<EventDTO>>();
                return eventAttendance != null ? Result<List<EventDTO>>.Ok(eventAttendance) : Result<List<EventDTO>>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<List<EventDTO>>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<List<EventDTO>>.NotFound(errorMessage);
                default:
                    return Result<List<EventDTO>>.InternalServerError(errorMessage);
            }
        }
        public async Task<Result<List<UserDTO>>> GetUsersForASingleEventAsync(int eventId)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/eventattendances/event/{eventId}/users");
            if (response.IsSuccessStatusCode)
            {
                List<UserDTO>? eventAttendance = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                return eventAttendance != null ? Result<List<UserDTO>>.Ok(eventAttendance) : Result<List<UserDTO>>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<List<UserDTO>>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<List<UserDTO>>.NotFound(errorMessage);
                default:
                    return Result<List<UserDTO>>.InternalServerError(errorMessage);
            }
        }
        public async Task<Result<EventAttendanceDTO>> UpdateEventAttendanceAsync(int id, EventAttendanceDTO eventAttendanceToUpdate)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync($"api/eventattendances/{id}", eventAttendanceToUpdate);
            if (response.IsSuccessStatusCode)
            {
                EventAttendanceDTO? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceDTO>();
                return eventAttendance != null ? Result<EventAttendanceDTO>.Ok(eventAttendance) : Result<EventAttendanceDTO>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<EventAttendanceDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<EventAttendanceDTO>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<EventAttendanceDTO>.Conflict(errorMessage);
                default:
                    return Result<EventAttendanceDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<EventAttendanceDTO>> DeleteEventAttendanceAsync(int id)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"api/eventattendances/{id}");
            if (response.IsSuccessStatusCode)
            {
                EventAttendanceDTO? eventAttendance = await response.Content.ReadFromJsonAsync<EventAttendanceDTO>();
                return eventAttendance != null ? Result<EventAttendanceDTO>.Ok(eventAttendance) : Result<EventAttendanceDTO>.InternalServerError("Failed to deserialize event attendance");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<EventAttendanceDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<EventAttendanceDTO>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<EventAttendanceDTO>.Conflict(errorMessage);
                default:
                    return Result<EventAttendanceDTO>.InternalServerError(errorMessage);
            }
        }
    }
}