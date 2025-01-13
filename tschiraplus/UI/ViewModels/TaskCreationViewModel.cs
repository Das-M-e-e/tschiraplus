using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    // Services
    private readonly ITaskService _taskService;
    private readonly MainTabViewModel _mainTabViewModel;

    // Bindings
    public bool IsLowPrio {get; set;}
    public bool IsHighPrio {get; set;}
    public bool IsMediumPrio {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    
    public string InitialStatus {get; set;}
    
    // Commands
    public ICommand CreateTaskCommand { get; }
    
    public TaskCreationViewModel(ITaskService taskService, MainTabViewModel mainTabViewModel)
    {
        _taskService = taskService;
        _mainTabViewModel = mainTabViewModel;
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
            DateTime.Today);
        _taskService.CreateTask(dto);
        _mainTabViewModel.SelectedTabIndex = 0;
        _mainTabViewModel.CloseCurrentTab();
    }
}