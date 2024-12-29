using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;
using Services.Repositories;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    // Services
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskService _taskService;

    // Bindings
    private bool _isLowPrio {get; set;}
    private bool _isHighPrio {get; set;}
    private bool _isMediumPrio {get; set;}
    private String _title {get; set;}
    private String _description {get; set;}
    
    // Commands
    public ICommand CreateTaskCommand { get; }
    
    public TaskCreationViewModel(ITaskRepository taskRepository, ITaskService taskService)
    {
        _taskService = taskService;
        CreateTaskCommand = new RelayCommand(CreateTask);
    }
    
    /// <summary>
    /// Creates a new TaskDto
    /// </summary>
    /// <returns>The TaskDto</returns>
    private TaskDto CreateTaskDTO()
    {
        TaskDto dto = new TaskDto()
        {
            TaskId = Guid.NewGuid(),
            Title = string.Empty,
            Description = string.Empty,
            Status = string.Empty,
            CreationDate = DateTime.Today
        };
        
        return dto;
    }

    /// <summary>
    /// Uses the _taskService to create a new task
    /// </summary>
    private void CreateTask()
    {
        TaskDto dto = _taskService.CreateTaskDto(
            _title,
            _description,
            _isLowPrio ? "Low" : _isMediumPrio ? "Medium" : "High",
            DateTime.Today);
        _taskService.TaskCreation(dto);
    }
}