using Cinema.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public class ShowtimeViewModel : ViewModelBase
    {
        private int _id;
        private int _movieId;
        private DateTime _time;
        private int _screenId;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public int MovieId
        {
            get => _movieId;
            set
            {
                _movieId = value;
                OnPropertyChanged();
            }
        }

        public DateTime Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public int ScreenId
        {
            get => _screenId;
            set
            {
                _screenId = value;
                OnPropertyChanged();
            }
        }

        public static explicit operator ShowtimeViewModel(ShowtimeDto dto) => new ShowtimeViewModel
        {
            Id = dto.Id,
            MovieId = dto.MovieId,
            Time = dto.Time,
            ScreenId = dto.ScreenId
        };

        public static explicit operator ShowtimeDto(ShowtimeViewModel vm) => new ShowtimeDto
        {
            Id = vm.Id,
            MovieId = vm.MovieId,
            Time = vm.Time,
            ScreenId = vm.ScreenId
        };
    }
}
