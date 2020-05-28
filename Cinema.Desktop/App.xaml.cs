using Cinema.Desktop.Model;
using Cinema.Desktop.View;
using Cinema.Desktop.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cinema.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CinemaApiService _service;
        private MainViewModel _mainViewModel;
        private LoginViewModel _loginViewModel;
        private MainWindow _mainView;
        private LoginWindow _loginView;
        private MovieEditorWindow _movieEditorView;
        private ShowtimeEditorWindow _showtimeEditorView;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new CinemaApiService(ConfigurationManager.AppSettings["baseAddress"]);

            _loginViewModel = new LoginViewModel(_service);

            _loginViewModel.LogintSucceeded += ViewModel_LoginSucceeded;
            _loginViewModel.LoginFailed += ViewModel_LoginFailed;
            _loginViewModel.MessageApplication += ViewModel_MessageApplication;

            _loginView = new LoginWindow
            {
                DataContext = _loginViewModel
            };

            _mainViewModel = new MainViewModel(_service);
            _mainViewModel.LogoutSucceeded += ViewModel_LogoutSucceeded;
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;

            _mainViewModel.StartingMovieEdit += ViewModel_StartingMovieEdit;
            _mainViewModel.FinishingMovieEdit += ViewModel_FinishingMovieEdit;
            _mainViewModel.StartingImageChange += ViewModel_StartingImageChange;

            _mainViewModel.StartingShowtimeEdit += ViewModel_StartingShowtimeEdit;
            _mainViewModel.FinishingShowtimeEdit += ViewModel_FinishingShowtimeEdit;

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _loginView.Show();
        }

        private void ViewModel_LoginSucceeded(object sender, EventArgs e)
        {
            _loginView.Hide();
            _mainView.Show();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Login failed!", "TodoList", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_LogoutSucceeded(object sender, EventArgs e)
        {
            _mainView.Hide();
            _loginView.Show();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Cinema", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_StartingMovieEdit(object sender, EventArgs e)
        {
            _movieEditorView = new MovieEditorWindow
            {
                DataContext = _mainViewModel
            };
            _movieEditorView.ShowDialog();
        }

        private void ViewModel_FinishingMovieEdit(object sender, EventArgs e)
        {
            if (_movieEditorView.IsActive)
            {
                _movieEditorView.Close();
            }
        }

        private async void ViewModel_StartingImageChange(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "Images|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog(_movieEditorView).GetValueOrDefault(false))
            {
                _mainViewModel.SelectedMovie.Poster = await File.ReadAllBytesAsync(dialog.FileName);
            }
        }

        private void ViewModel_StartingShowtimeEdit(object sender, EventArgs e)
        {
            _showtimeEditorView = new ShowtimeEditorWindow
            {
                DataContext = _mainViewModel
            };
            _showtimeEditorView.ShowDialog();
        }

        private void ViewModel_FinishingShowtimeEdit(object sender, EventArgs e)
        {
            if (_showtimeEditorView.IsActive)
            {
                _showtimeEditorView.Close();
            }
        }
    }
}
