using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFApp
{
    public partial class TaskListItem : UserControl
    {
        // Define the DependencyProperties for Title, Description, DueDate, TaskID, and TeamID
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TaskListItem), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(TaskListItem), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty DueDateProperty =
            DependencyProperty.Register("DueDate", typeof(string), typeof(TaskListItem), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TaskIDProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(TaskListItem), new PropertyMetadata(0));

        public static readonly DependencyProperty TeamIDProperty =
            DependencyProperty.Register("TeamId", typeof(int), typeof(TaskListItem), new PropertyMetadata(0));

        // Define the public properties for binding the UI to these dependency properties
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

        public int Id
        {
            get => (int)GetValue(TaskIDProperty);
            set => SetValue(TaskIDProperty, value);
        }

        public int TeamId
        {
            get => (int)GetValue(TeamIDProperty);
            set => SetValue(TeamIDProperty, value);
        }

        public TaskListItem()
        {
            InitializeComponent();
        }

        // Event to handle when a task is selected
        public event EventHandler<TaskSelectedEventArgs> TaskSelected;

        // Handle MouseDown to trigger the TaskSelected event
        private void TaskItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TaskSelected?.Invoke(this, new TaskSelectedEventArgs
            {
                Title = Title,
                Description = Description,
                DueDate = DueDate,
                Id = Id,
                TeamId = TeamId
            });
        }
    }
}

public class TaskSelectedEventArgs : EventArgs
{
    // Define properties to store the details of the selected task
    public string Title { get; set; }
    public string Description { get; set; }
    public string DueDate { get; set; }
    public int Id { get; set; }    // ID of the Task
    public int TeamId { get; set; }    // ID of the Team
}
