using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFApp
{
    public partial class TaskListItem : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TaskListItem), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(TaskListItem), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DueDateProperty =
            DependencyProperty.Register("DueDate", typeof(string), typeof(TaskListItem), new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public string DueDate
        {
            get => (string)GetValue(DueDateProperty);
            set => SetValue(DueDateProperty, value);
        }

        public TaskListItem()
        {
            InitializeComponent();
        }
        public event EventHandler<TaskSelectedEventArgs> TaskSelected;
        private void TaskItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Truyền task hiện tại khi nhấp
            TaskSelected?.Invoke(this, new TaskSelectedEventArgs
            {
                Title = Title,
                Description = Description,
                DueDate = DueDate
            });
        }
    }
}
public class TaskSelectedEventArgs : EventArgs
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string DueDate { get; set; }
}