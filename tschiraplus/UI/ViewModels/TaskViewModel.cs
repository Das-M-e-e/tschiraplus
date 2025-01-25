using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;

namespace UI.ViewModels;

public class TaskViewModel
{
    // Services
    private readonly TaskListViewModel _taskListViewModel;
    
    // Bindings
    public Guid TaskId { get; }
    public string Title { get; }
    public string Description { get; }
    public string? Status { get; }
    public DateTime? StartDate { get; }
    
    // Commands
    public ICommand DeleteTaskCommand { get; }

    public TaskViewModel(TaskDto task, TaskListViewModel taskListViewModel) //Konstruktor
    {
        TaskId = task.TaskId;
        Title = task.Title;
        Description = task.Description;
        Status = task.Status;
        StartDate = task.StartDate;

        _taskListViewModel = taskListViewModel;
        DeleteTaskCommand = new RelayCommand(DeleteTask);
    }

    /// <summary>
    /// Uses the _taskListViewModel to delete the task
    /// </summary>
    private void DeleteTask()
    {
        _taskListViewModel.DeleteTask(this);
    }
}