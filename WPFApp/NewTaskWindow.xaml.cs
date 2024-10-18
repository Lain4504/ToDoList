using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace WPFApp
{
    public partial class NewTaskWindow : Window
    {
        private readonly IToDoService _toDoService;
        private readonly int _teamId;
        public delegate void TaskAddedEventHandler(object sender, EventArgs e);
        public event TaskAddedEventHandler TaskAdded;
        // Parameterized constructor
        public NewTaskWindow(IToDoService toDoService, int teamId)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window_MouseLeftButtonDown);
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
                    NotificationWindow notificationWindow = new NotificationWindow("Please fill in all field.");
                    notificationWindow.Show();
                    return;
                }

                // Create a new ToDo item
                var newToDo = new ToDo
                {
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextbox.Text,
                    DueDate = PeriodDatePicker.SelectedDate ?? DateTime.Now,
                    IsCompleted = false 
                };

                // Call the service to add the ToDo for the specified team
                _toDoService.AddToDoForTeam(1, newToDo);

                TaskAdded?.Invoke(this, EventArgs.Empty);
                
                NotificationWindow notification = new NotificationWindow("Task added successfully");
                notification.Show();
                Close();
            }
            catch (Exception ex)
            {
                NotificationWindow notification = new NotificationWindow($"Error: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
