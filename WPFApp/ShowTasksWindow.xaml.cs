using System.Windows;
using BusinessObjects;

namespace WPFApp
{
    public partial class ShowTasksWindow : Window
    {
        private Team _team;

        public ShowTasksWindow(Team team)
        {
            InitializeComponent();
            _team = team;
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = _team.ToDos;
            TasksListBox.ItemsSource = _team.ToDos;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
