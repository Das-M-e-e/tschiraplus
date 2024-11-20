using System;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Core.Models;

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

    public TaskViewModel(TaskModel task, TaskListViewModel taskListViewModel)
    {
        TaskId = task.TaskId;
        Title = task.Title ?? "Unnamed Task";
        Description = task.Description ?? "...";
        Status = task.Status.ToString();
        CreationDate = task.CreationDate;

        _taskListViewModel = taskListViewModel;
        DeleteTaskCommand = new RelayCommand(DeleteTask);
    }

    private void DeleteTask()
    {
        _taskListViewModel.DeleteTask(this);
    }
}