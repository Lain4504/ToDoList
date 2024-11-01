using BusinessObjects;
using Services;
using System;
using System.Windows;

namespace WPFApp
{
    public partial class UpdateTaskCompletion : Window
    {
        private readonly IToDoService _toDoService;
        private readonly int _teamId;
        private readonly int _todoId;

        public event EventHandler TaskCompletionUpdated; // Declare an event

        public UpdateTaskCompletion(IToDoService toDoService, int teamId, int todoId)
        {
            InitializeComponent();
            _toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));
            _teamId = teamId;
            _todoId = todoId;

            LoadTaskDetails(); // Load task details
        }

        private void LoadTaskDetails()
        {
            var todo = _toDoService.GetToDoDetails(_teamId, _todoId);
            if (todo != null)
            {
                IsCompletedCheckBox.IsChecked = todo.IsCompleted;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Update task completion status
                _toDoService.UpdateTaskCompletionStatus(_teamId, _todoId, IsCompletedCheckBox.IsChecked ?? false);

                // Raise the event to notify that the task completion status has been updated
                TaskCompletionUpdated?.Invoke(this, EventArgs.Empty);

                NotificationWindow notification = new NotificationWindow("Task completion status updated successfully");
                notification.Show();
                Close();
            }
            catch (Exception ex)
            {
                NotificationWindow notification = new NotificationWindow($"Error: {ex.Message}");
                notification.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
