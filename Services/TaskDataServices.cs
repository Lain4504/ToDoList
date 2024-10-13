using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BusinessObjects;
using Newtonsoft.Json;

public class TaskDataService
{
    private readonly string _filePath;
    private readonly string folderName = "TaskTurner";
    private readonly string fileName = "tasks.json";

    public TaskDataService()
    {
        // Get the path to the app data
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        // Get the folder of the application in roaming
        string appFolder = Path.Combine(appDataPath, folderName);
        // Get the data folder inside the app
        string dataFolder = Path.Combine(appFolder, "data");

        // Check if the data folder exists
        if (!Directory.Exists(dataFolder))
        {
            // Create the directory if the folder doesn't exist
            Directory.CreateDirectory(dataFolder);
        }

        // Define the path to the json file
        _filePath = Path.Combine(dataFolder, fileName);

        // Ensure the json file exists
        InitializeFile();
    }

    private void InitializeFile()
    {
        // Check if the file exists
        if (!File.Exists(_filePath))
        {
            // Create a json file and add the structure
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<ToDo>()));
        }

        // For Debug purposes
        Process.Start(new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName),
            UseShellExecute = true
        });
    }
    public List<ToDo> LoadTasks()
    {
        // Read and deserialize the JSON file.
        string fileContent = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<List<ToDo>>(fileContent);
    }
    public void SaveTasks(List<ToDo> tasks)
    {
        // Serialize and write the list of tasks to the JSON file.
        string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
    public void AddTask(ToDo newTask)
    {
        // Generate a new Id number for the task
        newTask.Id = GenerateNewTaskId();

        // Load existing tasks from the JSON file
        var tasks = LoadTasks();

        // Add the new task to the list of tasks
        tasks.Add(newTask);

        // Save the updated list of tasks back to the JSON file
        SaveTasks(tasks);
    }
    public void UpdateTask(ToDo updateTask)
    {
        // Loading the tasks from the JSON file
        var tasks = LoadTasks();

        // Checking for the index of the task that matches the updateTask ID
        var taskIndex = tasks.FindIndex(t => t.Id == updateTask.Id);

        if (taskIndex != -1)
        {
            // Update the task at the found index
            tasks[taskIndex] = updateTask;

            // Save the updated task list back to the JSON file
            SaveTasks(tasks);
        }
        else
        {
            Console.WriteLine($"Task with ID {updateTask.Id} not found.");
        }
    }
    public void DeleteTasks(int taskId)
    {
        // Loading the tasks from the JSON file
        var tasks = LoadTasks();

        // Checking for the task with the matching ID and removing it
        var removedCount = tasks.RemoveAll(t => t.Id == taskId);

        // If no tasks were removed, print a message
        if (removedCount == 0)
        {
            Console.WriteLine($"Task with ID {taskId} not found.");
        }
        else
        {
            // Save the updated task list back to the JSON file
            SaveTasks(tasks);
        }
    }

    public int GenerateNewTaskId()
    {
        var tasks = LoadTasks();

        if (!tasks.Any())
            return 1;

        int maxId = tasks.Max(t => t.Id);
        return maxId + 1;
    }
  

}