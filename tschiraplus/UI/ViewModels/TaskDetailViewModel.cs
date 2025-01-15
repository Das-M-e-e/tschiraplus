using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services.DTOs;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskDetailViewModel : ViewModelBase
{
    // Services
    private readonly ITaskService _taskService;
    private TaskDto _taskDto;
    
    //Bindings
    private string? _title;

    public string Title
    {
        get => _title ?? string.Empty;
        set => this.RaiseAndSetIfChanged(ref _title, value);
        
    }

    private bool _isEditingTitle;

    public bool IsEditingTitle
    {
        get => _isEditingTitle;
        set => this.RaiseAndSetIfChanged(ref _isEditingTitle, value);
    }
    
    // Commands
    
    public ICommand StartEditingTitleCommand { get; set; }
    public ICommand SaveTitleCommand { get; set; }
    
    
    public TaskDetailViewModel(ITaskService taskService, Guid taskId)
    {
        _taskService = taskService;
        LoadTask(taskId);
        
        StartEditingTitleCommand = new RelayCommand(StartEditingTitle);
        SaveTitleCommand = new RelayCommand(SaveTitle);
        
    }

    /// <summary>
    /// Load the Task that has the given id
    /// </summary>
    /// <param name="taskId"></param>
    private void LoadTask(Guid taskId)
    {
        _taskDto = _taskService.GetTaskById(taskId);
        Title = _taskDto.Title;
    }

    /// <summary>
    /// Sets the Visibility of the Title
    /// </summary>
    private void StartEditingTitle()
    {
        IsEditingTitle = true;
    }

    /// <summary>
    /// Using the TaskService to update the task
    /// </summary>
    private void SaveTitle()
    {
        if (_taskDto.Title != Title)
        {
            _taskDto.Title = Title;
            _taskService.UpdateTask(_taskDto);
        }
        IsEditingTitle = false;
    }
    
    
}