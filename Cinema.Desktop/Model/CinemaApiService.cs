using Cinema.Persistence.DTO;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Desktop.Model
{
    public class CinemaApiService
    {
        private readonly HttpClient _client;

        public CinemaApiService(string baseAddress)
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<IEnumerable<MovieDto>> LoadMoviesAsync()
        {
            var response = await _client.GetAsync("api/Movies/");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<MovieDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task<IEnumerable<ShowtimeDto>> LoadShowtimesAsync(int movieId)
        {
            var response = await _client.GetAsync(
                QueryHelpers.AddQueryString("api/Showtimes/", "movieId", movieId.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<ShowtimeDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
    }
}
