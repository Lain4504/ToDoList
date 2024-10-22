using BusinessObjects;
using DataAccessLayer;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WPFApp.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly ToDoListContext _dbContext;

        public RegisterWindow()
        {
            InitializeComponent();
            _dbContext = new ToDoListContext(); // Initialize DB context
            RegisterButton.Click += RegisterButton_Click;
            // Uncomment if you have a Cancel button
            // CancelButton.Click += CancelButton_Click;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Collect input data
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string fullName = FullNameTextBox.Text.Trim();
            DateTime? dayOfBirth = DayOfBirthDatePicker.SelectedDate;
            string phone = PhoneTextBox.Text.Trim();

            // Validate inputs
            if (!ValidateInputs(username, email, password, confirmPassword))
            {
                return; // Early return if validation fails
            }

            // Hash the password
            string passwordHash = HashPassword(password);

            // Check if user exists
            if (IsUserExists(username, email))
            {
                MessageBox.Show("Username or Email already exists.");
                return;
            }

            // Create and save new user
            CreateUser(username, email, passwordHash, fullName, dayOfBirth, phone);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close window
        }

        #region Helper Methods

        private bool ValidateInputs(string username, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return false;
            }

            return true;
        }

        private bool IsUserExists(string username, string email)
        {
            return _dbContext.Users.Any(u => u.Username == username || u.Email == email);
        }

        private void CreateUser(string username, string email, string passwordHash, string fullName, DateTime? dayOfBirth, string phone)
        {
            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                FullName = fullName,
                DayOfBirth = dayOfBirth ?? DateTime.MinValue, // Use default if not provided
                Phone = phone,
                Role = GetRoleForUser(username)
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            MessageBox.Show("Registration successful!");
            Close();
        }

        private int GetRoleForUser(string username)
        {
            // Default role as User (0); Admin (1) based on conditions
            return username.Equals("admin", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Convert byte to hexadecimal string
                }

                return builder.ToString();
            }
        }

        #endregion
    }
}
