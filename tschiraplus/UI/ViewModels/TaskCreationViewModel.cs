using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    // Services
    private readonly ITaskService _taskService;
    private readonly TaskListViewModel _taskListViewModel;

    // Bindings
    public bool IsLowPrio {get; set;}
    public bool IsHighPrio {get; set;}
    public bool IsMediumPrio {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    
    public string InitialStatus {get; set;}
    
    // Commands
    public ICommand CreateTaskCommand { get; }
    
    public TaskCreationViewModel(ITaskService taskService, TaskListViewModel taskListViewModel)
    {
        _taskService = taskService;
        _taskListViewModel = taskListViewModel;
        
        CreateTaskCommand = new RelayCommand(CreateTask);
    }

    /// <summary>
    /// Uses the _taskService to create a new task
    /// </summary>
    private void CreateTask()
    {
        var dto = _taskService.CreateTaskDto(
            Title,
            Description,
            InitialStatus,
            "NotSet",
            DateTime.Today);
        _taskService.CreateTask(dto);
        _taskListViewModel.LoadTasks();
    }
}