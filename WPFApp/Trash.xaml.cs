using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WPFApp
{
    public partial class Trash // Thêm từ khóa partial
    {
        public Trash(){
            InitializeComponent();
        }

        public void DeleteTaskPermanently(DeletedTask task)
        {
            DeletedTasks.Remove(task);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }

    public class DeletedTask
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime DeletedAt { get; set; }

    }
}
