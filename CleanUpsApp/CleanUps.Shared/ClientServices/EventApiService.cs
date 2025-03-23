using CleanUps.Shared.DTOs;
using System.Net.Http.Json;

namespace CleanUps.Shared.ClientServices
{
    /// <summary>
    /// Provides methods for interacting with the event API endpoints.
    /// </summary>
    public class EventApiService
    {
        private readonly HttpClient _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventApiService"/> class.
        /// </summary>
        /// <param name="http">
        /// The <see cref="HttpClient"/> used for sending HTTP requests to the API, the DI in MauiProgram sets the HttpClient automatically.
        /// </param>
        public EventApiService(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Creates a new event by sending a POST request to the API.
        /// </summary>
        /// <param name="newEvent">
        /// The <see cref="EventDTO"/> representing the event to create.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the created <see cref="EventDTO"/>.
        /// </returns>
        public async Task<EventDTO> CreateEvent(EventDTO newEvent)
        {
            var response = await _http.PostAsJsonAsync("api/events", newEvent);
            return await response.Content.ReadFromJsonAsync<EventDTO>();
        }

        /// <summary>
        /// Retrieves all events by sending a GET request to the API.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing a <see cref="List{T}"/> of <see cref="EventDTO"/> objects.
        /// </returns>
        public async Task<List<EventDTO>> GetEvents()
        {
            return await _http.GetFromJsonAsync<List<EventDTO>>("api/events");
        }

        /// <summary>
        /// Retrieves an event by its identifier by sending a GET request to the API.
        /// </summary>
        /// <param name="id">
        /// The identifier of the event to retrieve.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the <see cref="EventDTO"/> if found.
        /// </returns>
        public async Task<EventDTO> GetEventById(int id)
        {
            return await _http.GetFromJsonAsync<EventDTO>($"api/events/{id}");
        }

        /// <summary>
        /// Updates an existing event by sending a PUT request to the API.
        /// </summary>
        /// <param name="id">
        /// The identifier of the event to update.
        /// </param>
        /// <param name="eventToUpdate">
        /// The <see cref="EventDTO"/> containing updated event details.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the updated <see cref="EventDTO"/>.
        /// </returns>
        public async Task<EventDTO> UpdateEvent(int id, EventDTO eventToUpdate)
        {
            var response = await _http.PutAsJsonAsync($"api/events/{id}", eventToUpdate);
            return await response.Content.ReadFromJsonAsync<EventDTO>();
        }

        /// <summary>
        /// Deletes an event by sending a DELETE request to the API.
        /// </summary>
        /// <param name="id">
        /// The identifier of the event to delete.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing the deleted <see cref="EventDTO"/>.
        /// </returns>
        public async Task<EventDTO> DeleteEvent(int id)
        {
            var response = await _http.DeleteAsync($"api/events/{id}");
            return await response.Content.ReadFromJsonAsync<EventDTO>();
        }
    }
}
