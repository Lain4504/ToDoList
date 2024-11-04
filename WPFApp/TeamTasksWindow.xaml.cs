using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for TeamTasksWindow.xaml
    /// </summary>
    public partial class TeamTasksWindow : Window
    {
        public TeamTasksWindow(List<string> tasks)
        {
            InitializeComponent();
            TasksListBox.ItemsSource = tasks;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
