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
    private readonly TaskService _taskService;
    private bool _isLowPrio {get; set;}
    private bool _isHighPrio {get; set;}
    private bool _isMediumPrio {get; set;}
    private String _title {get; set;}
    private String _description {get; set;}
    private DateTime _CreationDate {get; set;}
    public ICommand CreateTaskCommand { get; }
    
    
    
    public TaskCreationViewModel(TaskRepository taskRepository, TaskService taskService) //Konstrukto (evntl. unötig?)
    {
        _taskService = taskService;
        CreateTaskCommand = new RelayCommand(CreateTask);
    }
    
    //Erstellt eine TaskDTo
    private TaskDto CreateTaskDTO()
    {
        TaskDto dto = new TaskDto()
        {
            TaskId = Guid.NewGuid(),
            Title = _title,
            Description = _description,
            Status = _isLowPrio ? "Low" : _isMediumPrio ? "Medium" : "High",
            CreationDate = _CreationDate
        };
        return dto;
    }

    private void CreateTask()
    {
        TaskDto dto = CreateTaskDTO();
        _taskService.TaskCreation(dto);
    }
    
}