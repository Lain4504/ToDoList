using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    public partial class MainWindow : Window
    {
        private readonly IToDoService _toDoService;

        // Constructor không tham số
        public MainWindow() : this(new ToDoService(new ToDoRepository(new ToDoListContext())))
        {
            InitializeComponent();
        }

        // Constructor với tham số
        public MainWindow(IToDoService toDoService)
        {
            InitializeComponent();
            _toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));
            LoadTeamTasks(1); // Ví dụ ID đội, nên thay thế nó bằng cách chọn của người dùng hoặc logic khác
        }

        public void LoadTeamTasks(int teamID)
        {
            try
            {
                IEnumerable<ToDo> tasks = _toDoService.GetToDosForTeam(teamID);
                TaskListView.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private int _currentTaskID;
        private int _currentTeamID;

        private void TaskListItem_TaskSelected(object sender, TaskSelectedEventArgs e)
        {
            // Cập nhật dữ liệu cho Task Viewer
            TaskTitleTextBlock.Text = e.Title; // TextBlock cho tiêu đề task

            // Gán nội dung cho TextBlock bên trong ScrollViewer
            var taskDescriptionTextBlock = TaskDescriptionScrollViewer.Content as TextBlock;
            if (taskDescriptionTextBlock != null)
            {
                taskDescriptionTextBlock.Text = e.Description;
            }

            DueDateTextBlock.Text = $"Due: {e.DueDate}"; // TextBlock cho ngày hết hạn
            _currentTaskID = e.Id;
            _currentTeamID = e.TeamId;
        }

        // Sự kiện xóa task
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"Attempting to delete task with TeamID = {_currentTeamID}, TaskID = {_currentTaskID}");


            if (_currentTaskID > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _toDoService.DeleteToDoForTeam(_currentTeamID, _currentTaskID); // Gọi service để xóa task
                        MessageBox.Show("Task deleted successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Cập nhật danh sách tasks sau khi xóa
                        LoadTeamTasks(_currentTeamID);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No task selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // MainWindow.xaml.cs

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTitle = TaskSearchBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                try
                {
                    // Call the asynchronous search method
                    IEnumerable<ToDo> tasks = await _toDoService.GetToDoByTitleAsync(searchTitle,1);
                    TaskListView.ItemsSource = tasks; // Update the task list view with the search results
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                LoadTeamTasks(1); // Reload the original task list if the search box is empty
            }
        }

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of IToDoService
            IToDoService toDoService = new ToDoService(new ToDoRepository(new ToDoListContext()));

            // Set the appropriate team ID (for example, 1)
            int teamId = 1; // Replace with the actual team ID as necessary

            // Create and show the NewTaskWindow with the necessary parameters
            NewTaskWindow newTaskWindow = new NewTaskWindow(toDoService, teamId);
            newTaskWindow.ShowDialog(); // Open the window as a dialog
        }



    }
}
