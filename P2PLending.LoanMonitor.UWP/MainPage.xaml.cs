using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PaskoluKlubas.UWP.NewLoanWatcher.Exceptions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PaskoluKlubas.UWP.NewLoanWatcher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private PeriodicLoanChecker _loanChecker;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void StopMonitoring_Click(object sender, RoutedEventArgs e)
        {
            LoginBox.IsEnabled = true;
            PasswordBox.IsEnabled = true;
            StartMonitoring.IsEnabled = true;

            StartMonitoring.Visibility = Visibility.Visible;
            StopMonitoring.Visibility = Visibility.Collapsed;
            MonitoringProgressBar.Visibility = Visibility.Collapsed;
            
            _loanChecker?.Stop();
        }

        private async void StartMonitoring_Click(object sender, RoutedEventArgs e)
        {
            LoginBox.IsEnabled = false;
            PasswordBox.IsEnabled = false;
            StartMonitoring.IsEnabled = false;

            var toastMessenger = new PaskoluKlubasToastMessageRenderer();

            var issuerCfg = new LoanIssuerClientConfiguration
            {
                LoanIssuer = LoanIssuer.PaskoluKlubas,
                Login = LoginBox.Text,
                Password = PasswordBox.Password
            };

            _loanChecker = PeriodicLoanCheckerBuilder
                .SetLoanIssuer(issuerCfg)
                .CheckEvery(TimeSpan.FromMinutes(5))
                .CallOnNewLoans(toastMessenger.ShowToastMessage)
                .Build();

            try
            {
                StartMonitoring.Visibility = Visibility.Collapsed;
                StopMonitoring.Visibility = Visibility.Visible;
                MonitoringProgressBar.Visibility = Visibility.Visible;
                LoginFailedTextBlock.Visibility = Visibility.Collapsed;

                StopMonitoring.IsEnabled = true;
          
                await _loanChecker.StartAsync();
            }
            catch (LoginFailedException ex)
            {
                LoginBox.IsEnabled = true;
                PasswordBox.IsEnabled = true;
                StartMonitoring.IsEnabled = true;

                LoginFailedTextBlock.Visibility = Visibility.Visible;
                StartMonitoring.Visibility = Visibility.Visible;
                StopMonitoring.Visibility = Visibility.Collapsed;
                MonitoringProgressBar.Visibility = Visibility.Collapsed;

            }
        }

        private void LoginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LoginBox.Text.Length != 0 && PasswordBox.Password.Length != 0)
            {
                StartMonitoring.IsEnabled = true;
            }
            else
            {
                StartMonitoring.IsEnabled = false;
            }

            LoginFailedTextBlock.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text.Length != 0 && PasswordBox.Password.Length != 0)
            {
                StartMonitoring.IsEnabled = true;
            }
            else
            {
                StartMonitoring.IsEnabled = false;
            }

            LoginFailedTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
