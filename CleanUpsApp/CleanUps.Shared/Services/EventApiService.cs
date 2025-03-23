using CleanUps.Shared.DTOs;
using System.Net.Http.Json;

namespace CleanUps.Shared.Services
{
    public class EventApiService
    {
        private readonly HttpClient _http;

        public EventApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<EventDTO> CreateEvent(EventDTO newEvent)
        {
            var response = await _http.PostAsJsonAsync("api/events", newEvent);
            return await response.Content.ReadFromJsonAsync<EventDTO>();
        }

        public async Task<List<EventDTO>> GetEvents()
        {
            return await _http.GetFromJsonAsync<List<EventDTO>>("api/events");
        }


        // Add other methods for update/delete
    }
}
