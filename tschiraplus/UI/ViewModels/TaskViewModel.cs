using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Core.Models;

namespace UI.ViewModels;

public class TaskViewModel
{
    private readonly TaskListViewModel _taskListViewModel;
    public ICommand DeleteTaskCommand { get; }
    
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Tag { get; set; }
    public string User { get; set; }

    public TaskViewModel(TaskModel task, TaskListViewModel taskListViewModel)
    {
        _taskListViewModel = taskListViewModel;
        DeleteTaskCommand = new RelayCommand(DeleteTask);
        TaskId = task.TaskId;
        Title = task.Title;
        Description = task.Description;
        Tag = "Wichtig";
        User = "Franzi";
    }

    public void DeleteTask()
    {
        _taskListViewModel.DeleteTask(this);
    }
}