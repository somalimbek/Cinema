using Cinema.Persistence;
using Cinema.Persistence.DTO;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net;
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

        #region Authentication

        public async Task<bool> LoginAsync(string name, string password)
        {
            LoginDto user = new LoginDto
            {
                UserName = name,
                Password = password
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Login", user);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsync("api/Account/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        #endregion

        #region Movie

        public async Task<IEnumerable<MovieDto>> LoadMoviesAsync()
        {
            var response = await _client.GetAsync("api/Movies/");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<MovieDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task CreateMovieAsync(MovieDto movie)
        {
            var response = await _client.PostAsJsonAsync("api/movies/", movie);
            movie.Id = (await response.Content.ReadAsAsync<MovieDto>()).Id;

            if (!response.IsSuccessStatusCode)
            {
                throw new NetworkException("Service returned response: " + response.StatusCode);
            }
        }

        #endregion

        #region Showtimes

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

        #endregion

        #region Screens

        public async Task<ScreenDto> LoadScreenAsync(int showtimeId)
        {
            var response = await _client.GetAsync(
                QueryHelpers.AddQueryString("api/Screens/", "showtimeId", showtimeId.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ScreenDto>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        #endregion

        #region Seats

        public async Task<IEnumerable<SeatDto>> LoadSeatsAsync(int showtimeId)
        {
            var response = await _client.GetAsync(
                QueryHelpers.AddQueryString("api/Seats/", "showtimeId", showtimeId.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<SeatDto>>();
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        #endregion
    }
}
