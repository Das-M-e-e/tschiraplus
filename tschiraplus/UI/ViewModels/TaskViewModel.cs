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
    
    public string? Priority { get; }
    public DateTime CreationDate { get; }
    
    // Commands
    public ICommand DeleteTaskCommand { get; }
    public ICommand OpenTaskDetailCommand { get; }

    public TaskViewModel(TaskDto task, TaskListViewModel taskListViewModel) 
    {
        TaskId = task.TaskId;
        Title = task.Title;
        Description = task.Description;
        Status = task.Status;
        Priority = task.Priority;
        CreationDate = task.CreationDate;

        _taskListViewModel = taskListViewModel;
        DeleteTaskCommand = new RelayCommand(DeleteTask);
        OpenTaskDetailCommand = new RelayCommand(OpenTaskDetails);
    }

    /// <summary>
    /// Uses the _taskListViewModel to delete the task
    /// </summary>
    private void DeleteTask()
    {
        _taskListViewModel.DeleteTask(this);
    }

    /// <summary>
    /// Uses the _taskListViewModel to open the TaskDetailView
    /// </summary>
    private void OpenTaskDetails()
    {
        _taskListViewModel.OpenTaskDetails(TaskId);
    }
}