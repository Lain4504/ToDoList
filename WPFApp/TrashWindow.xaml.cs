using System;
using BusinessObjects;
using Repositories;
using Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFApp
{
    public partial class TrashWindow : Window
    {
        private readonly ToDoService _todoService;
        private int _currentTeamId;

        public ObservableCollection<ToDo> DeletedTodos { get; set; }

        public TrashWindow(ToDoService todoService, int teamId)
        {
            InitializeComponent();
            _todoService = todoService;
            _currentTeamId = teamId;
            DeletedTodos = new ObservableCollection<ToDo>(_todoService.GetDeletedTodos());
            DeletedTodosListView.DataContext = this;
        }

        public TrashWindow(ToDoService todoService)
        {
            InitializeComponent();
            _todoService = todoService;
            DeletedTodos = new ObservableCollection<ToDo>(_todoService.GetDeletedTodos());
            DeletedTodosListView.DataContext = this;
        }

        public TrashWindow()
        {
            InitializeComponent();
        }

        private void RestoreToDo(int todoId, int teamId)
        {
            _todoService.RestoreToDo(todoId, teamId);
            ReloadDeletedTodos();
        }

        private void PermanentlyDeleteTodo(int todoId)
        {
            _todoService.PermanentlyDeleteTodo(todoId);
            ReloadDeletedTodos();
        }

        private void ReloadDeletedTodos()
        {
            DeletedTodos.Clear();
            foreach (var todo in _todoService.GetDeletedTodos())
            {
                DeletedTodos.Add(todo);
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && int.TryParse(button.Tag.ToString(), out int todoId))
            {
                RestoreToDo(todoId, _currentTeamId);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && int.TryParse(button.Tag.ToString(), out int todoId))
            {
                PermanentlyDeleteTodo(todoId);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadDeletedTodos();
        }

        private void DeletedTodosListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            this.DragMove();
        }
    }
}
