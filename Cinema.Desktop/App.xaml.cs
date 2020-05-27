using Cinema.Desktop.Model;
using Cinema.Desktop.View;
using Cinema.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            _mainViewModel.MessageApplication += ViewModel_MessageApplication;

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

    }
}
