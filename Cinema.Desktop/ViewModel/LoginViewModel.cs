using Cinema.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Cinema.Desktop.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly CinemaApiService _model;
        private bool _isLoading;

        public DelegateCommand LoginCommand { get; private set; }

        public String UserName { get; set; }

        public Boolean IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler LogintSucceeded;

        public event EventHandler LoginFailed;

        public LoginViewModel(CinemaApiService model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            UserName = String.Empty;
            IsLoading = false;

            LoginCommand = new DelegateCommand(_ => !IsLoading, param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null)
                return;

            try
            {
                IsLoading = true;
                bool result = await _model.LoginAsync(UserName, passwordBox.Password);
                IsLoading = false;

                if (result)
                    OnLoginSuccess();
                else
                    OnLoginFailed();
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private void OnLoginSuccess()
        {
            LogintSucceeded?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
