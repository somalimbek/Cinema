using Cinema.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.Desktop.ViewModel
{
    public enum DisplayStatus
    {
        Free,
        Booked,
        Sold,
        Selected
    }

    public class SeatViewModel : ViewModelBase
    {
        private DisplayStatus _displayStatus;

        public int Id { get; set; }

        public int ShowtimeId { get; set; }

        public int RowNumber { get; set; }

        public int SeatNumber { get; set; }

        public SeatStatus Status { get; set; }

        public DisplayStatus DisplayStatus
        {
            get { return _displayStatus; }
            set
            {
                if (_displayStatus != value)
                {
                    _displayStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public DelegateCommand SelectSeatCommand { get; set; }
    }
}
