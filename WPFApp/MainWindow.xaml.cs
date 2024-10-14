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
            LoadTeamTasks(3); // Ví dụ ID đội, nên thay thế nó bằng cách chọn của người dùng hoặc logic khác
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
    }
}
