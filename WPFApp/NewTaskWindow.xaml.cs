using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Windows;

namespace WPFApp
{
    public partial class NewTaskWindow : Window
    {
        private readonly IToDoService _toDoService;
        private readonly int _teamId;

        // Parameterized constructor
        public NewTaskWindow(IToDoService toDoService, int teamId)
        {
            InitializeComponent();
            _toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));
            _teamId = teamId; // Store the team ID
        }
        // Event handler for the Save button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTextbox.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                // Create a new ToDo item
                var newToDo = new ToDo
                {
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextbox.Text,
                    DueDate = PeriodDatePicker.SelectedDate ?? DateTime.Now,
                    IsCompleted = false // You can set this based on your logic
                };

                // Call the service to add the ToDo for the specified team
                _toDoService.AddToDoForTeam(1, newToDo);

                // Optionally show a success message or close the window
                MessageBox.Show("Task added successfully.");
                Close();
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show an error message)
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Optionally handle the Back button to close the window
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
