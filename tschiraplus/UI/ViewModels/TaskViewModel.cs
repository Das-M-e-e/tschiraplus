using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;

namespace UI.ViewModels;

public class TaskViewModel
{
    private readonly TaskListViewModel _taskListViewModel;
    public ICommand DeleteTaskCommand { get; }
    
    public Guid TaskId { get; }
    public string Title { get; }
    public string Description { get; }
    public string? Status { get; }
    public DateTime CreationDate { get; }

    public TaskViewModel(TaskDto task, TaskListViewModel taskListViewModel)
    {
        TaskId = task.TaskId;
        Title = task.Title;
        Description = task.Description;
        Status = task.Status;
        CreationDate = task.CreationDate;

        _taskListViewModel = taskListViewModel;
        DeleteTaskCommand = new RelayCommand(DeleteTask);
    }

    private void DeleteTask()
    {
        _taskListViewModel.DeleteTask(this);
    }
}