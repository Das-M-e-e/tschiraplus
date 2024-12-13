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
    private DateTime _creationDate {get; set;}
    public ICommand CreateTaskCommand { get; }
    
    
    
    public TaskCreationViewModel(TaskRepository taskRepository, TaskService taskService) //Konstrukto (evntl. unötig?)
    {
        _taskService = taskService;
        CreateTaskCommand = new RelayCommand(CreateTask);
    }
    
    //Methode die dem ButtonCommand gegeben wird
    private void CreateTask()
    {
        TaskDto dto = _taskService.CreateTaskDto(
            _title,
            _description,
            _isLowPrio ? "Low" : _isMediumPrio ? "Medium" : "High",
            _creationDate);
        _taskService.TaskCreation(dto);
    }
    
}