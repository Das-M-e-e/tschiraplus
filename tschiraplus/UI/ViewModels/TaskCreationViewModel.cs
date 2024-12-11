using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Services.DTOs;
using Services.Repositories;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskService _taskService;

    public TaskCreationViewModel(ITaskRepository taskRepository, ITaskService taskService)
    {
        _taskRepository = taskRepository;
        _taskService = taskService;
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
}