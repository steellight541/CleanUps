using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;

namespace CleanUps.Shared.ClientServices
{
    public class EventApiService
    {
        private readonly HttpClient _http;

        public EventApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Result<EventDTO>> CreateEventAsync(EventDTO newEvent)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/events", newEvent);
            if (response.IsSuccessStatusCode)
            {
                EventDTO? eventDto = await response.Content.ReadFromJsonAsync<EventDTO>();
                return eventDto != null ? Result<EventDTO>.Created(eventDto) : Result<EventDTO>.InternalServerError("Failed to deserialize event");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<EventDTO>.BadRequest(errorMessage);
                default:
                    return Result<EventDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<List<EventDTO>>> GetEventsAsync()
        {
            HttpResponseMessage response = await _http.GetAsync("api/events");
            if (response.IsSuccessStatusCode)
            {
                List<EventDTO>? events = await response.Content.ReadFromJsonAsync<List<EventDTO>>();
                return events != null ? Result<List<EventDTO>>.Ok(events) : Result<List<EventDTO>>.InternalServerError("Failed to deserialize events");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return Result<List<EventDTO>>.NoContent();
                default:
                    return Result<List<EventDTO>>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<EventDTO>> GetEventByIdAsync(int id)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/events/{id}");
            if (response.IsSuccessStatusCode)
            {
                EventDTO? eventDto = await response.Content.ReadFromJsonAsync<EventDTO>();
                return eventDto != null ? Result<EventDTO>.Ok(eventDto) : Result<EventDTO>.InternalServerError("Failed to deserialize event");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<EventDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<EventDTO>.NotFound(errorMessage);
                default:
                    return Result<EventDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<EventDTO>> UpdateEventAsync(int id, EventDTO eventToUpdate)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync($"api/events/{id}", eventToUpdate);
            if (response.IsSuccessStatusCode)
            {
                EventDTO? eventDto = await response.Content.ReadFromJsonAsync<EventDTO>();
                return eventDto != null ? Result<EventDTO>.Ok(eventDto) : Result<EventDTO>.InternalServerError("Failed to deserialize event");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<EventDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<EventDTO>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<EventDTO>.Conflict(errorMessage);
                default:
                    return Result<EventDTO>.InternalServerError(errorMessage);
            }
        }

        public async Task<Result<EventDTO>> DeleteEventAsync(int id)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"api/events/{id}");
            if (response.IsSuccessStatusCode)
            {
                EventDTO? eventDto = await response.Content.ReadFromJsonAsync<EventDTO>();
                return eventDto != null ? Result<EventDTO>.Ok(eventDto) : Result<EventDTO>.InternalServerError("Failed to deserialize event");
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Result<EventDTO>.BadRequest(errorMessage);
                case HttpStatusCode.NotFound:
                    return Result<EventDTO>.NotFound(errorMessage);
                case HttpStatusCode.Conflict:
                    return Result<EventDTO>.Conflict(errorMessage);
                default:
                    return Result<EventDTO>.InternalServerError(errorMessage);
            }
        }
    }
}