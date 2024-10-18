using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFApp
{
   
    public partial class TaskWindow : Window
    {
        private readonly IToDoService _toDoService;

        // Constructor không tham số
        public TaskWindow() : this(new ToDoService(new ToDoRepository(new ToDoListContext())))
        {
            InitializeComponent();
        }

        // Constructor với tham số
        public TaskWindow(IToDoService toDoService)
        {
            InitializeComponent();
            _toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));
            LoadTeamTasks(1); 
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Giảm cửa sổ
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized; // Max
            else
                this.WindowState = WindowState.Normal; // Trở lại trạng thái bình thường
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Đóng cửa sổ
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
            if (_currentTaskID > 0)
            {
                // Hiển thị cửa sổ xác nhận xóa
                ConfirmationWindow confirmationWindow = new ConfirmationWindow();
                confirmationWindow.Owner = this; // Thiết lập chủ sở hữu của cửa sổ xác nhận là TaskWindow
                confirmationWindow.ShowDialog(); // Hiển thị cửa sổ dưới dạng dialog

                // Kiểm tra nếu người dùng xác nhận xóa
                if (confirmationWindow.IsConfirmed)
                {
                    try
                    {
                        _toDoService.DeleteToDoForTeam(_currentTeamID, _currentTaskID); // Gọi service để xóa task

                        // Hiển thị thông báo thành công qua NotificationWindow
                        NotificationWindow notification = new NotificationWindow("Task deleted successfully!");
                        notification.ShowDialog();

                        // Xóa dữ liệu hiển thị trên Task Viewer
                        TaskTitleTextBlock.Text = string.Empty; // Xóa tiêu đề task
                        var taskDescriptionTextBlock = TaskDescriptionScrollViewer.Content as TextBlock;
                        if (taskDescriptionTextBlock != null)
                        {
                            taskDescriptionTextBlock.Text = string.Empty; // Xóa nội dung task
                        }
                        DueDateTextBlock.Text = string.Empty; // Xóa ngày hết hạn

                        // Reset ID của task hiện tại về 0
                        _currentTaskID = 0;
                        // Cập nhật danh sách tasks sau khi xóa
                        LoadTeamTasks(_currentTeamID);
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị thông báo lỗi qua NotificationWindow
                        NotificationWindow notification = new NotificationWindow($"Error deleting task: {ex.Message}");
                        notification.ShowDialog();
                    }
                }
            }
            else
            {
                // Hiển thị thông báo khi không có task được chọn
                NotificationWindow notification = new NotificationWindow("No task selected for deletion.");
                notification.ShowDialog();
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTitle = TaskSearchBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                try
                {
                    IEnumerable<ToDo> tasks = await _toDoService.GetToDoByTitleAsync(searchTitle,1);
                    TaskListView.ItemsSource = tasks; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                LoadTeamTasks(1); 
            }
        }

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            int teamId = 1;
            NewTaskWindow newTaskWindow = new NewTaskWindow(_toDoService, teamId);

            // Lắng nghe sự kiện TaskAdded để cập nhật giao diện task detail
            newTaskWindow.TaskAdded += (s, args) =>
            {
                var taskArgs = args as TaskAddedEventArgs;
                if (taskArgs != null)
                {
                    var newTask = taskArgs.NewTask;

                    // Cập nhật task detail UI
                    TaskTitleTextBlock.Text = newTask.Title;
                    var taskDescriptionTextBlock = TaskDescriptionScrollViewer.Content as TextBlock;
                    if (taskDescriptionTextBlock != null)
                    {
                        taskDescriptionTextBlock.Text = newTask.Description;
                    }
                    DueDateTextBlock.Text = $"Due: {newTask.DueDate}";
                    _currentTaskID = newTask.Id;
                    _currentTeamID = teamId;
                }

                // Tải lại danh sách tasks
                LoadTeamTasks(teamId);
            };

            newTaskWindow.ShowDialog();
        }

        private void UpdateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTaskID > 0)
            {
                int teamId = 1;
                UpdateTask updateWindow = new UpdateTask(_toDoService, teamId, _currentTaskID);
                updateWindow.TaskUpdated += (s, args) =>
                {
                    LoadTeamTasks(teamId);
                    var updateTask = _toDoService.GetToDoDetails(1, _currentTaskID);
                    if (updateTask != null)
                    {
                        TaskTitleTextBlock.Text = updateTask.Title;
                        var taskDescriptionBlock = TaskDescriptionScrollViewer.Content as TextBlock;
                        if (taskDescriptionBlock != null)
                        {
                            taskDescriptionBlock.Text = updateTask.Description;
                        }
                        DueDateTextBlock.Text = $"Due: {updateTask.DueDate}";
                    }
                };
                updateWindow.ShowDialog();
            }
            else
            {
                NotificationWindow notification = new NotificationWindow("Please choose a task before prress update.");
                notification.ShowDialog();
            }
            
        }
        private void TaskSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTitle = TaskSearchBox.Text.Trim();
            if (string.IsNullOrEmpty(searchTitle))
            {
                LoadTeamTasks(1);
            }
        }
        private void BinButton_Click(object sender, RoutedEventArgs e)
        {
            Trash trash = new Trash();
            trash.Show();
            this.Close();
        }

    }
}
