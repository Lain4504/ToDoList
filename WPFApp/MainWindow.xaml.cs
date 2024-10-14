using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Windows;

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
        private void TaskListItem_TaskSelected(object sender, TaskSelectedEventArgs e)
        {
            // Cập nhật dữ liệu cho Task Viewer
            TaskTitleTextBlock.Text = e.Title; // TextBlock cho tiêu đề task
            TaskDescriptionScrollViewer.Content = e.Description; // ScrollViewer cho mô tả task
            DueDateTextBlock.Text = $"Due: {e.DueDate}"; // TextBlock cho ngày hết hạn
        }
    }
}
