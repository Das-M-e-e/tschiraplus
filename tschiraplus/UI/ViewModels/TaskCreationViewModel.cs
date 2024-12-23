using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;
using Services.Repositories;
using Services.TaskServices;
using ReactiveUI;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskService _taskService;

    private bool _isLowPrio {get; set;}
    private bool _isHighPrio {get; set;}
    private bool _isMediumPrio {get; set;}
    private String _title {get; set;}
    private String _description {get; set;}
   
    public ICommand CreateTaskCommand { get; }
    
    public TaskCreationViewModel(ITaskRepository taskRepository, ITaskService taskService)
    {
        _taskService = taskService;
        CreateTaskCommand = new RelayCommand(CreateTask);
    }
    
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