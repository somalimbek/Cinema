using Cinema.Persistence.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public class MovieViewModel : ViewModelBase, IDataErrorInfo
    {
        private int _id;
        private string _title;
        private string _director;
        private string _cast;
        private string _storyline;
        private int _runtime;
        private byte[] _poster;
        private DateTime _added;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Title):
                        if (string.IsNullOrEmpty(Title))
                            error = "Title cannot be empty.";
                        else if (Title.Length > 100)
                            error = "Title cannot be longer than 100 characters.";
                        break;
                    case nameof(Director):
                        if (string.IsNullOrEmpty(Director))
                            error = "Director cannot be empty.";
                        else if (Director.Length > 50)
                            error = "Director cannot be longer than 100 characters.";
                        break;
                    case nameof(Cast):
                        if (string.IsNullOrEmpty(Cast))
                            error = "Cast cannot be empty.";
                        break;
                    case nameof(Storyline):
                        if (string.IsNullOrEmpty(Storyline))
                            error = "Storyline cannot be empty.";
                        break;
                    case nameof(Runtime):
                        if (Runtime < 0)
                            error = "Runtime cannot be negative.";
                        break;
                }
                return error;
            }
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Director
        {
            get => _director;
            set
            {
                _director = value;
                OnPropertyChanged();
            }
        }

        public string Cast
        {
            get => _cast;
            set
            {
                _cast = value;
                OnPropertyChanged();
            }
        }

        public string Storyline
        {
            get => _storyline;
            set
            {
                _storyline = value;
                OnPropertyChanged();
            }
        }

        public int Runtime
        {
            get => _runtime;
            set
            {
                _runtime = value;
                OnPropertyChanged();
            }
        }

        public byte[] Poster
        {
            get => _poster;
            set
            {
                _poster = value;
                OnPropertyChanged();
            }
        }

        public DateTime Added
        {
            get => _added;
            set
            {
                _added = value;
                OnPropertyChanged();
            }
        }

        public String Error => String.Empty;

        public static explicit operator MovieViewModel(MovieDto dto) => new MovieViewModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Director = dto.Director,
            Cast = dto.Cast,
            Storyline = dto.Storyline,
            Runtime = dto.Runtime,
            Poster = dto.Poster,
            Added = dto.Added
        };

        public static explicit operator MovieDto(MovieViewModel vm) => new MovieDto
        {
            Id = vm.Id,
            Title = vm.Title,
            Director = vm.Director,
            Cast = vm.Cast,
            Storyline = vm.Storyline,
            Runtime = vm.Runtime,
            Poster = vm.Poster,
            Added = vm.Added
        };
    }
}
