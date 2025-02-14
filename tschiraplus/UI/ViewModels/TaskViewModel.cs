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
    private Guid TaskId { get; }
    public string Title { get; }
    public string Description { get; }
    public bool IsDone { get; set; }
    
    // Commands
    public ICommand OpenTaskDetailCommand { get; }
    public ICommand ToggleTaskDoneCommand { get; }

    public TaskViewModel(TaskDto task, TaskListViewModel taskListViewModel) 
    {
        TaskId = task.TaskId;
        Title = task.Title;
        Description = task.Description.Replace("\r\n", " ");
        IsDone = task.Status == "Done";

        _taskListViewModel = taskListViewModel;
        OpenTaskDetailCommand = new RelayCommand(OpenTaskDetails);
        ToggleTaskDoneCommand = new RelayCommand(ToggleTaskDone);
    }

    /// <summary>
    /// Uses the _taskListViewModel to open the TaskDetailView
    /// </summary>
    private void OpenTaskDetails()
    {
        _taskListViewModel.OpenTaskDetails(TaskId);
    }

    /// <summary>
    /// Sets this tasks status to "Done"
    /// </summary>
    private void ToggleTaskDone()
    {
        _taskListViewModel.ToggleTaskDone(TaskId);
    }
}