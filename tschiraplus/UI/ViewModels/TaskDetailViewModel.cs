using System;
using System.Collections.ObjectModel;
using System.Linq;
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

    private SelectionItemViewModel _status;
    public SelectionItemViewModel Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }
    
    private SelectionItemViewModel _priority;
    public SelectionItemViewModel Priority
    {
        get => _priority;
        set => this.RaiseAndSetIfChanged(ref _priority, value);
    }

    private string? _startDate;
    public string StartDate
    {
        get => _startDate ?? string.Empty;
        set => this.RaiseAndSetIfChanged(ref _startDate, value);
    }
    
    private bool _isEditingStartDate;
    public bool IsEditingStartDate
    {
        get => _isEditingStartDate;
        set => this.RaiseAndSetIfChanged(ref _isEditingStartDate, value);
    }
    
        
    public string? Description {get; set;}
    
    public ObservableCollection<SelectionItemViewModel> StatusList { get; set; } = [];
    public ObservableCollection<SelectionItemViewModel> PriorityList { get; set; } = [];
    
    // Commands
    
    public ICommand StartEditingTitleCommand { get; set; }
    public ICommand StartEditingStartDateCommand { get; set; }
    public ICommand SaveTitleCommand { get; set; }
    public ICommand SaveDescriptionCommand { get; set; }
    
    public TaskDetailViewModel(ITaskService taskService, Guid taskId)
    {
        _taskService = taskService;
        LoadStatusList();
        LoadPriorityList();
        LoadTask(taskId);
        
        StartEditingTitleCommand = new RelayCommand(StartEditingTitle);
        SaveDescriptionCommand = new RelayCommand(SaveDescription);
        SaveTitleCommand = new RelayCommand(SaveTitle);
        StartEditingStartDateCommand = new RelayCommand(StartEditingStartDate);
    }

    /// <summary>
    /// Load the Task that has the given id
    /// </summary>
    /// <param name="taskId"></param>
    private void LoadTask(Guid taskId)
    {
        _taskDto = _taskService.GetTaskById(taskId);
        Title = _taskDto.Title;
        Status = StatusList.FirstOrDefault(s => (string)s.Tag! == _taskDto.Status)
            ?? StatusList.First();
        Priority = PriorityList.FirstOrDefault(s => (string)s.Tag! == _taskDto.Priority)
            ?? PriorityList.First();
        Description = _taskDto.Description;
        
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
    
    private void LoadStatusList()
    {
        StatusList.Add(new SelectionItemViewModel{Name = "Backlog", Tag ="Backlog", ColorCode = "#d3d3d3"});
        StatusList.Add(new SelectionItemViewModel{Name = "Ready", Tag ="Ready", ColorCode = "#87cefa"});
        StatusList.Add(new SelectionItemViewModel{Name = "In Progress", Tag ="InProgress", ColorCode = "#ffa500"});
        StatusList.Add(new SelectionItemViewModel{Name = "In Review", Tag ="InReview", ColorCode = "#9370db"});
        StatusList.Add(new SelectionItemViewModel{Name = "Done", Tag ="Done", ColorCode = "#32cd32"});
    }

    private void LoadPriorityList() 
    {
        PriorityList.Add(new SelectionItemViewModel{Name = "Not Set", Tag ="NotSet", ColorCode = "#d3d3d3"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Low", Tag ="Low", ColorCode = "#95D8A1"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Medium", Tag ="Medium", ColorCode = "#8FCDF3"});
        PriorityList.Add(new SelectionItemViewModel{Name = "High", Tag ="High", ColorCode = "#EBA29A"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Critical", Tag ="Critical", ColorCode = "#DD5550"});
    }

    public void SetStatus(string? status)
    {
        Status = StatusList.First(s => s.Name == status);
        _taskDto.Status = Status.Name.Replace(" ","");
        _taskService.UpdateTask(_taskDto);
    }

    public void SetPriority(string? priority)
    {
        Priority = PriorityList.First(s => s.Name == priority);
        _taskDto.Priority = Priority.Name.Replace(" ","");
        _taskService.UpdateTask(_taskDto);
    }

    private void StartEditingStartDate()
    {
        IsEditingStartDate = true;
    }

    private void SaveDescription()
    {
        if (_taskDto.Description != Description)
        {
            _taskDto.Description = Description;
            _taskService.UpdateTask(_taskDto);
        }
    }
}