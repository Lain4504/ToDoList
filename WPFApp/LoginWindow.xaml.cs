using System;
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

        public LoginWindow()
        {
            InitializeComponent();
            _dbContext = new ToDoListContext();

            // Event handlers for the buttons
            LoginButton.Click += LoginButton_Click;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Validate login using the database
            if (ValidateLogin(username, password))
            {
                MessageBox.Show("Login successful!");
                // Navigate to the main application window (to be implemented)
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the login window
        }

        #region Helper Methods

        private bool ValidateLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username or password cannot be empty.");
                return false;
            }

            // Hash the entered password to compare with the stored password hash
            string passwordHash = HashPassword(password);

            // Find user by username and password hash
            var user = _dbContext.Users
                .FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);

            return user != null; // Returns true if user exists, false otherwise
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

        #endregion
    }
}
