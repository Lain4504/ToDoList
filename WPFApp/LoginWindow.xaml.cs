using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;

namespace WPFApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly ToDoListContext _dbContext;
        private bool _hasShownError;

        public LoginWindow()
        {
            InitializeComponent();
            _dbContext = new ToDoListContext();

            // Event handlers for the buttons
            LoginButton.Click += LoginButton_Click;
            RegisterButton.Click += RegisterButton_Click;
            _hasShownError = false; // Initialize the flag
        }
        private bool _notificationShown = false; // Add a flag to track notification status

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (ValidateLogin(username, password))
            {
                if (!_notificationShown)
                {
                    _notificationShown = true; // Set flag to true to prevent multiple notifications
                    NotificationWindow successNotification = new NotificationWindow("Login successful!");

                    successNotification.Closed += (s, args) =>
                    {
                        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == HashPassword(password));
                        if (user != null)
                        {
                            OpenTaskWindow(user.UserId);
                            this.Close();
                        }
                    };

                    this.Hide();
                    successNotification.ShowDialog();
                }
            }
            else
            {
                if (!_notificationShown)
                {
                    _notificationShown = true;

                    NotificationWindow errorNotification = new NotificationWindow("Invalid username or password. Please try again.");

                    errorNotification.ShowDialog();
                }
            }
        }

        #region Helper Methods

        private bool ValidateLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _hasShownError = true;
                return false;
            }

            string passwordHash = HashPassword(password);

            var user = _dbContext.Users
                .FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);

            return user != null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute hash returns a byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to string
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private void OpenTaskWindow(int userID)
        {
            TeamWindow teamWindow = new TeamWindow(new TeamService(new TeamRepository(new ToDoListContext())), userID);
            teamWindow.Show();
            this.Close(); 
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is RegisterWindow)
                {
                    window.Activate(); 
                    return; 
                }
            }

            RegisterWindow registerWindow = new RegisterWindow();
            this.Close();
            registerWindow.Show();
        }

        #endregion

        private void UsernameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }
    }
}
