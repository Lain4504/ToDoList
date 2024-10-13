using BusinessObjects;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WPFApp.ViewModel;
public class TaskViewModel : INotifyPropertyChanged
{
    // This is used to interact with the underlying data storage.
    private readonly TaskDataService _taskDataService;

    // Private field holding the collection of task models.
    // ObservableCollection is used because it provides notifications when items get added, removed, or the entire list is refreshed.
    private ObservableCollection<ToDo> _tasks;

    // Event declaration for PropertyChanged.
    // This is used in the context of INotifyPropertyChanged to notify the UI when a property value changes.
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Public property for tasks.
    /// Provides access to the _tasks field and notifies the UI when the collection changes.
    /// </summary>
    public ObservableCollection<ToDo> Tasks
    {
        get => _tasks;
        set
        {
            _tasks = value;
            OnPropertyChanged(nameof(Tasks));
        }
    }

    // Constructor
    public TaskViewModel()
    {
        _taskDataService = new TaskDataService(); // Initializing the TaskDataService.
        LoadTasks(); // Load tasks when the ViewModel is initialized.
    }

    // LoadTasks: This method loads the list of tasks from the TaskDataService and updates the Tasks collection.
    private void LoadTasks()
    {
        var taskList = _taskDataService.LoadTasks();
        Tasks = new ObservableCollection<ToDo>(taskList);
    }

    // AddNewTask: This method is responsible for adding a new task.
    // It calls the AddTask method of TaskDataService to save the new task and then reloads the task list to update the UI.
    public void AddNewTask(ToDo newTask)
    {
        _taskDataService.AddTask(newTask);
        LoadTasks(); // Reload tasks to reflect the new addition.
    }

    // UpdateTask: This method is used to update an existing task.
    // It calls the UpdateTask method of TaskDataService with the updated task data and then reloads the task list.
    public void UpdateTask(ToDo updateTask)
    {
        _taskDataService.UpdateTask(updateTask);
        LoadTasks();
    }

    // DeleteTask: This method is used to delete a task based on its ID.
    // It calls the DeleteTask method of TaskDataService and then reloads the task list to ensure the UI is up-to-date.
    public void DeleteTask(int taskId)
    {
        _taskDataService.DeleteTasks(taskId);
        LoadTasks();
    }

    // OnPropertyChanged: This method is used to notify the view of property value changes.
    // When a property value changes, the PropertyChanged event is raised with the name of the property, updating the binding in the UI.
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}