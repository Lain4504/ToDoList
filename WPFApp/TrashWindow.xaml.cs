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
        private readonly IToDoService _todoService; 
        private int _currentTeamId;

        public ObservableCollection<ToDo> DeletedTodos { get; set; }

        public TrashWindow(IToDoService todoService, int teamId)
        {
            InitializeComponent();
            _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));
            _currentTeamId = teamId;
            DeletedTodos = new ObservableCollection<ToDo>(_todoService.GetDeletedTodos());
            DeletedTodosItemsControl.DataContext = this;
        }



        private void RestoreToDo(int todoId, int teamId)
        {
            _todoService.RestoreToDo(todoId, teamId);
            ReloadDeletedTodos();
        }

        private void PermanentlyDeleteTodo(int teamId, int todoId)
        {
            _todoService.PermanentlyDeleteTodo(teamId, todoId);
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
                ReloadDeletedTodos();
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && int.TryParse(button.Tag.ToString(), out int todoId))
            {
                
                _todoService.PermanentlyDeleteTodo(_currentTeamId, todoId);
                ReloadDeletedTodos();
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
        private void BinButton_Click(object sender, RoutedEventArgs e)
        {
            var TrashWindow = new TrashWindow(_todoService, _currentTeamId);
            TrashWindow.Show(); 
        }

    }
}
