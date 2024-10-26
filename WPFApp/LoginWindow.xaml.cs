﻿using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using DataAccessLayer;

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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            bool isNotificationOpen = Application.Current.Windows.OfType<NotificationWindow>().Any();

            if (ValidateLogin(username, password))
            {
                if (!isNotificationOpen)
                {
                    NotificationWindow successNotification = new NotificationWindow("Login successful!");
                    successNotification.Show();
                }
                OpenTaskWindow();
            }
            else
            {
                if (!isNotificationOpen)
                {
                    NotificationWindow errorNotification = new NotificationWindow("Invalid username or password. Please try again.");
                    errorNotification.Show();
                    _hasShownError = true;
                }
            }
        }


        #region Helper Methods

        private bool ValidateLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username or password cannot be empty.");
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

        private void OpenTaskWindow()
        {
            TeamWindow teamWindow = new TeamWindow();
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
