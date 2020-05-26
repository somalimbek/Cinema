using Cinema.Desktop.Model;
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
        private readonly CinemaApiService _service;

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

        public DelegateCommand SelectCommand { get; private set; }

        public DelegateCommand RefreshMoviesCommand { get; private set; }

        public MainViewModel(CinemaApiService service)
        {
            _service = service;

            RefreshMoviesCommand = new DelegateCommand(_ => LoadMoviesAsync());
            SelectCommand = new DelegateCommand(param => LoadShowtimesAsync(param as MovieDto));
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
    }
}
