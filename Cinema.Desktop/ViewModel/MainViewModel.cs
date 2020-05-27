using Cinema.Desktop.Model;
using Cinema.Persistence;
using Cinema.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<MovieDto> _movies;
        private ObservableCollection<ShowtimeDto> _showtimes;
        private ObservableCollection<SeatDto> _seats;
        private readonly CinemaApiService _service;

        private string _screenName = "Screen: ";
        private int _numberOfRows;
        private int _seatsPerRow;

        public ObservableCollection<MovieDto> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ShowtimeDto> Showtimes
        {
            get => _showtimes;
            set
            {
                _showtimes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SeatDto> Seats
        {
            get => _seats;
            set
            {
                _seats = value;
                OnPropertyChanged();
            }
        }

        public string ScreenName
        {
            get => _screenName;
            set
            {
                _screenName = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfRows
        {
            get => _numberOfRows;
            set
            {
                _numberOfRows = value;
                OnPropertyChanged();
            }
        }

        public int SeatsPerRow
        {
            get => _seatsPerRow;
            set
            {
                _seatsPerRow = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand SelectMovieCommand { get; private set; }

        public DelegateCommand SelectShowtimeCommand { get; private set; }

        public DelegateCommand RefreshMoviesCommand { get; private set; }

        public MainViewModel(CinemaApiService service)
        {
            _service = service;

            RefreshMoviesCommand = new DelegateCommand(_ => LoadMoviesAsync());
            SelectMovieCommand = new DelegateCommand(param => LoadShowtimesAsync(param as MovieDto));
            SelectShowtimeCommand = new DelegateCommand(param => LoadSeatsAsync(param as ShowtimeDto));
        }

        public async void LoadMoviesAsync()
        {
            try
            {
                Movies = new ObservableCollection<MovieDto>(await _service.LoadMoviesAsync());
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public async void LoadShowtimesAsync(MovieDto movie)
        {
            if (movie is null)
                return;
            try
            {
                Showtimes = new ObservableCollection<ShowtimeDto>(await _service.LoadShowtimesAsync(movie.Id));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public async void LoadSeatsAsync(ShowtimeDto showtime)
        {
            if (showtime is null)
                return;

            try
            {
                var screen = await _service.LoadScreenAsync(showtime.Id);
                ScreenName = "Screen: " + screen.Name;
                NumberOfRows = screen.NumberOfRows;
                SeatsPerRow = screen.SeatsPerRow;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }

            try
            {
                Seats = new ObservableCollection<SeatDto>(await _service.LoadSeatsAsync(showtime.Id));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }
    }
}
