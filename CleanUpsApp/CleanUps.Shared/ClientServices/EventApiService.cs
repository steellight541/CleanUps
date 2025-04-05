using CleanUps.Shared.DTOs.Events;
using CleanUps.Shared.ErrorHandling;
using System.Net;
using System.Net.Http.Json;

namespace CleanUps.Shared.ClientServices
{
    public class EventApiService
    {
        private readonly HttpClient _http;

        public EventApiService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("CleanupApi");
        }

        public async Task<Result<List<EventResponse>>> GetAllEventsAsync()
        {
            HttpResponseMessage response = await _http.GetAsync("api/events");
            if (response.IsSuccessStatusCode)
            {
                List<EventResponse>? events = await response.Content.ReadFromJsonAsync<List<EventResponse>>();
                return events != null ? Result<List<EventResponse>>.Ok(events) : Result<List<EventResponse>>.InternalServerError("Failed to deserialize events");
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

        public async Task<Result<EventResponse>> GetEventByIdAsync(int id)
        {
            HttpResponseMessage response = await _http.GetAsync($"api/events/{id}");
            if (response.IsSuccessStatusCode)
            {
                EventResponse? eventDto = await response.Content.ReadFromJsonAsync<EventResponse>();
                return eventDto != null ? Result<EventResponse>.Ok(eventDto) : Result<EventResponse>.InternalServerError("Failed to deserialize event");
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

        public async Task<Result<EventResponse>> CreateEventAsync(CreateEventRequest createRequest)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/events", createRequest);
            if (response.IsSuccessStatusCode)
            {
                EventResponse? createdEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                return createdEvent != null ? Result<EventResponse>.Created(createdEvent) : Result<EventResponse>.InternalServerError("Failed to deserialize event");
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

        public async Task<Result<EventResponse>> UpdateEventAsync(int id, UpdateEventRequest updateRequest)
        {
            HttpResponseMessage response = await _http.PutAsJsonAsync($"api/events/{id}", updateRequest);
            if (response.IsSuccessStatusCode)
            {
                EventResponse? updatedEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                return updatedEvent != null ? Result<EventResponse>.Ok(updatedEvent) : Result<EventResponse>.InternalServerError("Failed to deserialize event");
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

        public async Task<Result<EventResponse>> DeleteEventAsync(int eventId)
        {
            HttpResponseMessage response = await _http.DeleteAsync($"api/events/{eventId}");
            if (response.IsSuccessStatusCode)
            {
                EventResponse? deletedEvent = await response.Content.ReadFromJsonAsync<EventResponse>();
                return deletedEvent != null ? Result<EventResponse>.Ok(deletedEvent) : Result<EventResponse>.InternalServerError("Failed to deserialize event");
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
    }
}