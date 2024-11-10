using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Services.Repositories;

namespace UI.ViewModels;


public class TaskListViewModel
{
    private readonly TaskRepository _taskRepository;
    public ObservableCollection<TaskViewModel> Tasks { get; } = new();
    public ICommand AddRandomTaskCommand { get; }

    public TaskListViewModel(TaskRepository taskRepository)
    {
        _taskRepository = taskRepository;

        AddRandomTaskCommand = new RelayCommand(AddRandomTask);
        
        LoadTasks();
    }

    private void LoadTasks()
    {
        Tasks.Clear();
        var allTasks = _taskRepository.GetAllTasks();
        foreach (var task in allTasks)
        {
            Tasks.Add(new TaskViewModel(task, this));
        }
    }

    private void AddRandomTask()
    {
        var newTask = new TaskModel
        {
            TaskId = Guid.NewGuid(),
            Title = "Random Task " + new Random().Next(100),
            Description = "This is a randomly generated task",
            CreationDate = DateTime.Now
        };

        _taskRepository.AddTask(newTask);
        Tasks.Add(new TaskViewModel(newTask, this));
    }

    public void DeleteTask(TaskViewModel task)
    {
        Tasks.Remove(task);
        _taskRepository.DeleteTask(task.TaskId);
    }
}