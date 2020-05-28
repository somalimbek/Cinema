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
        private ObservableCollection<ShowtimeViewModel> _showtimes;
        private ObservableCollection<ScreenDto> _screens;
        private ObservableCollection<SeatViewModel> _seats;

        private MovieViewModel _selectedMovie;
        private ShowtimeViewModel _selectedShowtime;

        private string _screenName = _screenLabel;
        private int _numberOfRows;
        private int _seatsPerRow;

        private string _infoString = "";

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

        public List<MovieViewModel> MoviesForCombo
        {
            get => Movies.ToList();
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

        public ObservableCollection<ShowtimeViewModel> Showtimes
        {
            get => _showtimes;
            set
            {
                _showtimes = value;
                OnPropertyChanged();
            }
        }

        public ShowtimeViewModel SelectedShowtime
        {
            get => _selectedShowtime;
            set
            {
                _selectedShowtime = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ScreenDto> Screens
        {
            get => _screens;
            set
            {
                _screens = value;
                OnPropertyChanged();
            }
        }

        public List<ScreenDto> ScreensForCombo
        {
            get => Screens.ToList();
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

        public string InfoString
        {
            get => _infoString;
            set
            {
                _infoString = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand SelectMovieCommand { get; private set; }

        public DelegateCommand SelectShowtimeCommand { get; private set; }

        public DelegateCommand SellSelectedSeatsCommand { get; set; }

        public DelegateCommand RefreshSeatsCommand { get; private set; }

        public DelegateCommand AddMovieCommand { get; private set; }

        public DelegateCommand SaveMovieEditCommand { get; private set; }

        public DelegateCommand CancelMovieEditCommand { get; private set; }

        public DelegateCommand ChangeImageCommand { get; private set; }

        public DelegateCommand RefreshMoviesCommand { get; private set; }

        public DelegateCommand AddShowtimeCommand { get; private set; }

        public DelegateCommand SaveShowtimeEditCommand { get; private set; }

        public DelegateCommand CancelShowtimeEditCommand { get; private set; }

        public DelegateCommand LogoutCommand { get; private set; }

        public event EventHandler LogoutSucceeded;

        public event EventHandler StartingMovieEdit;

        public event EventHandler FinishingMovieEdit;

        public event EventHandler StartingImageChange;

        public event EventHandler StartingShowtimeEdit;

        public event EventHandler FinishingShowtimeEdit;

        public MainViewModel(CinemaApiService service)
        {
            _selectedSeats = new HashSet<SeatViewModel>();
            _screens = new ObservableCollection<ScreenDto>();

            _service = service;

            RefreshMoviesCommand = new DelegateCommand(_ => LoadMoviesAsync());
            LogoutCommand = new DelegateCommand(_ => LogoutAsync());

            SelectMovieCommand = new DelegateCommand(param => LoadShowtimesAsync(SelectedMovie));
            SelectShowtimeCommand = new DelegateCommand(param => LoadSeatsAsync(param as ShowtimeViewModel));
            SellSelectedSeatsCommand = new DelegateCommand(_ => SellSelectedSeats());
            RefreshSeatsCommand = new DelegateCommand(_ => !(SelectedShowtime is null), _ => LoadSeatsAsync(SelectedShowtime));

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

            AddShowtimeCommand = new DelegateCommand(_ => !(SelectedMovie is null), _ => AddShowtime());
            SaveShowtimeEditCommand = new DelegateCommand(_ => SaveShowtimeEdit());
            CancelShowtimeEditCommand = new DelegateCommand(_ => CancelShowtimeEdit());
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
                Movies = new ObservableCollection<MovieViewModel>(
                    (await _service.LoadMoviesAsync())
                    .Select(movie => (MovieViewModel)movie));
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
                Showtimes = new ObservableCollection<ShowtimeViewModel>(
                    (await _service.LoadShowtimesAsync(movie.Id))
                    .Select(showtime => (ShowtimeViewModel)showtime));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        public async void AddShowtime()
        {
            var newShowtime = new ShowtimeViewModel
            {
                MovieId = SelectedMovie.Id,
                Time = DateTime.Now,
                ScreenId = 1
            };
            Showtimes.Add(newShowtime);
            SelectedShowtime = newShowtime;
            Screens = new ObservableCollection<ScreenDto>(await _service.LoadScreensAsync());
            StartEditShowtime();
        }

        public void StartEditShowtime()
        {
            StartingShowtimeEdit?.Invoke(this, EventArgs.Empty);
        }

        private void CancelShowtimeEdit()
        {
            if (SelectedShowtime is null)
                return;

            if (SelectedShowtime.Id == 0)
            {
                Showtimes.Remove(SelectedShowtime);
                SelectedShowtime = null;
            }

            FinishingShowtimeEdit?.Invoke(this, EventArgs.Empty);
        }

        private async void SaveShowtimeEdit()
        {
            try
            {
                var showtimeDto = (ShowtimeDto)SelectedShowtime;
                await _service.CreateShowtimeAsync(showtimeDto);
                SelectedShowtime.Id = showtimeDto.Id;
                FinishingShowtimeEdit?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})\nMake sure the created showtime is not overlapping with other showtimes.");
            }
        }

        #endregion

        #region Seats

        public async void LoadSeatsAsync(ShowtimeViewModel showtime)
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
                    InfoString = "";
                    _selectedSeats.Add(seat);
                    seat.DisplayStatus = DisplayStatus.Selected;
                    break;
                case DisplayStatus.Booked:
                    InfoString = "Customer name: " + seat.CustomerName + " | Phone: " + seat.CustomerPhoneNumber;
                    _selectedSeats.Add(seat);
                    seat.DisplayStatus = DisplayStatus.Selected;
                    break;
                case DisplayStatus.Sold:
                    InfoString = "";
                    break;
                case DisplayStatus.Selected:
                    InfoString = "";
                    _selectedSeats.Remove(seat);
                    seat.DisplayStatus = (DisplayStatus)seat.Status;
                    break;
                default:
                    InfoString = "";
                    break;
            }
        }

        public async void SellSelectedSeats()
        {
            var seatsToSell = new List<SeatDto>();
            foreach (var seat in _selectedSeats)
            {
                seatsToSell.Add(new SeatDto
                {
                    Id = seat.Id,
                    ShowtimeId = seat.ShowtimeId,
                    RowNumber = seat.RowNumber,
                    SeatNumber = seat.SeatNumber,
                    Status = SeatStatus.Sold
                });
            }

            try
            {
                await _service.SellSeats(seatsToSell);
                _selectedSeats = new HashSet<SeatViewModel>();
                LoadSeatsAsync(SelectedShowtime);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        #endregion
    }
}
