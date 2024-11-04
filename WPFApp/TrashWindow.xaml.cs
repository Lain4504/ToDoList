using System;
using BusinessObjects;
using Repositories;
using Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataAccessLayer;

namespace WPFApp
{
    public partial class TrashWindow : Window
    {
        private readonly IToDoService _todoService; 
        private int _currentTeamId;
        private int _teamId;
        private readonly ToDoListContext dbContext = new ToDoListContext();

        public ObservableCollection<ToDo> DeletedTodos { get; set; }

        public TrashWindow(IToDoService todoService, int teamId)
        {
            InitializeComponent();
            _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));
            _currentTeamId = teamId;
            DeletedTodos = new ObservableCollection<ToDo>(_todoService.GetDeletedTodos());
            DeletedTodosItemsControl.DataContext = this;
        }


        private void RestoreToDoCobehind(int todoId, int teamId)
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
            // Bước 1: Kiểm tra kiểu của sender và Tag
    if (sender is Button button && button.Tag is int itemId)
            {
                // Kiểm tra xem itemId có giá trị không
                if (itemId > 0)
                {
                    // Bước 2: Tìm item trong cơ sở dữ liệu
                    var itemToRestore = dbContext.ToDos.FirstOrDefault(t => t.Id == itemId);

                    if (itemToRestore != null)
                    {
                        // Bước 3: Gọi phương thức RestoreToDo nếu itemToRestore không null
                        RestoreToDoCobehind(itemToRestore.Id, itemToRestore.TeamId);

                        // Bước 4: Gọi phương thức ReloadDeletedTodos
                        //ReloadDeletedTodos();

                        MessageBox.Show("Mục đã được khôi phục thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy mục trong cơ sở dữ liệu với ID: " + itemId);
                    }
                }
                else
                {
                    MessageBox.Show("Giá trị của itemId không hợp lệ: " + itemId);
                }
            }
            else
            {
                MessageBox.Show("Sender không phải là Button hoặc Tag không phải kiểu int.");
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
