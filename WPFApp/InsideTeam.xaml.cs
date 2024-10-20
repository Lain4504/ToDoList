using DataAccessLayer;
using Repositories;
using Services;
using System.Windows;

namespace WPFApp
{
    public partial class InsideTeam : Window
    {
        public int TeamId { get; private set; }

        public InsideTeam(
            //int teamId
            )
        {
            InitializeComponent();
            TeamId = 1; // Khởi tạo teamId từ tham số truyền vào
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ TaskWindow và truyền teamId
            TaskWindow taskWindow = new TaskWindow(new ToDoService(new ToDoRepository(new ToDoListContext())), TeamId);
            taskWindow.Show();
        }
    }
}
