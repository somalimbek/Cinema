using Cinema.Desktop.Model;
using Cinema.Persistence;
using Cinema.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const string _screenLabel = "Screen: ";

        private readonly CinemaApiService _service;
        private ObservableCollection<MovieViewModel> _movies;
        private ObservableCollection<ShowtimeDto> _showtimes;
        private ObservableCollection<SeatViewModel> _seats;

        private MovieViewModel _selectedMovie;

        private string _screenName = _screenLabel;
        private int _numberOfRows;
        private int _seatsPerRow;

        private HashSet<SeatViewModel> _selectedSeats;

        public ObservableCollection<MovieViewModel> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public MovieViewModel SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
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

        public ObservableCollection<SeatViewModel> Seats
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

        public DelegateCommand AddMovieCommand { get; private set; }

        public DelegateCommand SaveMovieEditCommand { get; private set; }

        public DelegateCommand CancelMovieEditCommand { get; private set; }

        public DelegateCommand ChangeImageCommand { get; private set; }

        public DelegateCommand RefreshMoviesCommand { get; private set; }

        public DelegateCommand LogoutCommand { get; private set; }

        public event EventHandler LogoutSucceeded;

        public event EventHandler StartingMovieEdit;

        public event EventHandler FinishingMovieEdit;

        public event EventHandler StartingImageChange;

        public MainViewModel(CinemaApiService service)
        {
            _selectedSeats = new HashSet<SeatViewModel>();

            _service = service;

            SelectMovieCommand = new DelegateCommand(param => LoadShowtimesAsync(SelectedMovie));
            SelectShowtimeCommand = new DelegateCommand(param => LoadSeatsAsync(param as ShowtimeDto));

            AddMovieCommand = new DelegateCommand(_ => AddMovie());
            SaveMovieEditCommand = new DelegateCommand(
                _ => string.IsNullOrEmpty(SelectedMovie?[nameof(MovieViewModel.Title)])
                && string.IsNullOrEmpty(SelectedMovie?[nameof(MovieViewModel.Director)])
                && string.IsNullOrEmpty(SelectedMovie?[nameof(MovieViewModel.Cast)])
                && string.IsNullOrEmpty(SelectedMovie?[nameof(MovieViewModel.Storyline)])
                && string.IsNullOrEmpty(SelectedMovie?[nameof(MovieViewModel.Runtime)]),
                _ => SaveMovieEdit()
                );
            CancelMovieEditCommand = new DelegateCommand(_ => CancelMovieEdit());
            ChangeImageCommand = new DelegateCommand(_ => StartingImageChange?.Invoke(this, EventArgs.Empty));

            RefreshMoviesCommand = new DelegateCommand(_ => LoadMoviesAsync());
            LogoutCommand = new DelegateCommand(_ => LogoutAsync());
        }

        #region Authentication

        private async void LogoutAsync()
        {
            try
            {
                await _service.LogoutAsync();
                OnLogoutSuccess();
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private void OnLogoutSuccess()
        {
            LogoutSucceeded?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Movies

        public async void LoadMoviesAsync()
        {
            try
            {
                Movies = new ObservableCollection<MovieViewModel>((await _service.LoadMoviesAsync()).Select(movie => (MovieViewModel)movie));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public void AddMovie()
        {
            var newMovie = new MovieViewModel
            {
                Title = "New Movie",
                Runtime = 100,
                Added = DateTime.Now
            };
            Movies.Add(newMovie);
            SelectedMovie = newMovie;
            StartEditMovie();
        }

        private void StartEditMovie()
        {
            StartingMovieEdit?.Invoke(this, EventArgs.Empty);
        }

        private void CancelMovieEdit()
        {
            if (SelectedMovie is null)
                return;

            if (SelectedMovie.Id == 0)
            {
                Movies.Remove(SelectedMovie);
                SelectedMovie = null;
            }

            FinishingMovieEdit?.Invoke(this, EventArgs.Empty);
        }

        private async void SaveMovieEdit()
        {
            try
            {
                var movieDto = (MovieDto)SelectedMovie;
                await _service.CreateMovieAsync(movieDto);
                SelectedMovie.Id = movieDto.Id;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
            FinishingMovieEdit?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Showtimes

        public async void LoadShowtimesAsync(MovieViewModel movie)
        {
            if (movie is null || movie.Id == 0)
            {
                Showtimes = null;
                Seats = null;
                ScreenName = _screenLabel;
                return;
            }
            try
            {
                Showtimes = new ObservableCollection<ShowtimeDto>(await _service.LoadShowtimesAsync(movie.Id));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        #endregion

        #region Seats

        public async void LoadSeatsAsync(ShowtimeDto showtime)
        {
            if (showtime is null || showtime.Id == 0)
            {
                Seats = null;
                ScreenName = _screenLabel;
                return;
            }
            try
            {
                var screen = await _service.LoadScreenAsync(showtime.Id);
                ScreenName = _screenLabel + screen.Name;
                NumberOfRows = screen.NumberOfRows;
                SeatsPerRow = screen.SeatsPerRow;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }

            try
            {
                Seats = new ObservableCollection<SeatViewModel>((await _service.LoadSeatsAsync(showtime.Id))
                    .Select(dto => new SeatViewModel
                    {
                        Id = dto.Id,
                        ShowtimeId = dto.ShowtimeId,
                        RowNumber = dto.RowNumber,
                        SeatNumber = dto.SeatNumber,
                        Status = dto.Status,
                        DisplayStatus = (DisplayStatus)dto.Status,
                        CustomerName = dto.CustomerName,
                        CustomerPhoneNumber = dto.CustomerPhoneNumber,
                        SelectSeatCommand = new DelegateCommand(param => SelectSeat(param as SeatViewModel))
                    })
                    .ToList());
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public void SelectSeat(SeatViewModel seat)
        {
            if (seat is null)
                return;

            switch (seat.DisplayStatus)
            {
                case DisplayStatus.Free:
                    _selectedSeats.Add(seat);
                    seat.DisplayStatus = DisplayStatus.Selected;
                    break;
                case DisplayStatus.Booked:
                    //TODO: Write out booking info
                    _selectedSeats.Add(seat);
                    seat.DisplayStatus = DisplayStatus.Selected;
                    break;
                case DisplayStatus.Sold:
                    break;
                case DisplayStatus.Selected:
                    _selectedSeats.Remove(seat);
                    seat.DisplayStatus = (DisplayStatus)seat.Status;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
