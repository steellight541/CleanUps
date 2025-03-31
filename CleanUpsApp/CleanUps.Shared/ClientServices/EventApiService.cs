using CleanUps.Shared.DTOs;
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

        public async Task<EventDTO> CreateEventAsync(EventDTO newEvent)
        {
            var response = await _http.PostAsJsonAsync("api/events", newEvent);
            return await response.Content.ReadFromJsonAsync<EventDTO>();
        }

        public async Task<List<EventDTO>> GetEventsAsync()
        {
            return await _http.GetFromJsonAsync<List<EventDTO>>("api/events");
        }

        public async Task<EventDTO> GetEventByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<EventDTO>($"api/events/{id}");
        }

        public async Task<EventDTO> UpdateEventAsync(int id, EventDTO eventToUpdate)
        {
            var response = await _http.PutAsJsonAsync($"api/events/{id}", eventToUpdate);
            return await response.Content.ReadFromJsonAsync<EventDTO>();
        }

        public async Task<EventDTO> DeleteEventAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/events/{id}");
            return await response.Content.ReadFromJsonAsync<EventDTO>();
        }
    }
}
