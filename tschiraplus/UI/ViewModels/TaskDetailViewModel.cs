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

    private DateTimeOffset? _startDate;
    public DateTimeOffset? StartDate
    {
        get => _startDate;
        set => this.RaiseAndSetIfChanged(ref _startDate, value);
    }
    
    private DateTimeOffset? _dueDate;
    public DateTimeOffset? DueDate
    {
        get => _dueDate;
        set => this.RaiseAndSetIfChanged(ref _dueDate, value);
    }
    
    private bool _isEditingStartDate;
    public bool IsEditingStartDate
    {
        get => _isEditingStartDate;
        set => this.RaiseAndSetIfChanged(ref _isEditingStartDate, value);
    }
    
    private bool _isEditingDueDate;
    public bool IsEditingDueDate
    {
        get => _isEditingDueDate;
        set => this.RaiseAndSetIfChanged(ref _isEditingDueDate, value);
    }
    
    private bool _isEditingDescription;
    public bool IsEditingDescription
    {
        get => _isEditingDescription;
        set => this.RaiseAndSetIfChanged(ref _isEditingDescription, value);
    }
    
    public string Username { get; set; }
        
    public string? Description {get; set;}
    
    public ObservableCollection<SelectionItemViewModel> StatusList { get; set; } = [];
    public ObservableCollection<SelectionItemViewModel> PriorityList { get; set; } = [];
    
    // Commands
    public ICommand StartEditingTitleCommand { get; set; }
    public ICommand StartEditingStartDateCommand { get; set; }
    public ICommand StartEditingDueDateCommand { get; set; }
    public ICommand SaveTitleCommand { get; set; }
    public ICommand SaveDescriptionCommand { get; set; }
    public ICommand AddTaskUserCommand { get; set; }
    
    public TaskDetailViewModel(ITaskService taskService, Guid taskId)
    {
        _taskService = taskService;
        LoadStatusList();
        LoadPriorityList();
        LoadTask(taskId);
        
        StartEditingTitleCommand = new RelayCommand(() => IsEditingTitle = true);
        SaveDescriptionCommand = new RelayCommand(SaveDescription);
        SaveTitleCommand = new RelayCommand(SaveTitle);
        StartEditingStartDateCommand = new RelayCommand(() => IsEditingStartDate = true);
        StartEditingDueDateCommand = new RelayCommand(() => IsEditingDueDate = true);
        AddTaskUserCommand = new RelayCommand(AddTaskUser);
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
        StartDate = _taskDto.StartDate.HasValue ? new DateTimeOffset(_taskDto.StartDate.Value) : null;
        DueDate = _taskDto.DueDate.HasValue ? new DateTimeOffset(_taskDto.DueDate.Value) : null;
        Description = _taskDto.Description;
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
    
    /// <summary>
    /// Fills the StatusList with preset status options for the UI
    /// </summary>
    private void LoadStatusList()
    {
        StatusList.Add(new SelectionItemViewModel{Name = "Backlog", Tag ="Backlog", ColorCode = "#d3d3d3"});
        StatusList.Add(new SelectionItemViewModel{Name = "Ready", Tag ="Ready", ColorCode = "#87cefa"});
        StatusList.Add(new SelectionItemViewModel{Name = "In Progress", Tag ="InProgress", ColorCode = "#ffa500"});
        StatusList.Add(new SelectionItemViewModel{Name = "In Review", Tag ="InReview", ColorCode = "#9370db"});
        StatusList.Add(new SelectionItemViewModel{Name = "Done", Tag ="Done", ColorCode = "#32cd32"});
    }

    /// <summary>
    /// Fills the PriorityList with preset priorities options for the UI
    /// </summary>
    private void LoadPriorityList() 
    {
        PriorityList.Add(new SelectionItemViewModel{Name = "Not Set", Tag ="NotSet", ColorCode = "#d3d3d3"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Low", Tag ="Low", ColorCode = "#95D8A1"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Medium", Tag ="Medium", ColorCode = "#8FCDF3"});
        PriorityList.Add(new SelectionItemViewModel{Name = "High", Tag ="High", ColorCode = "#EBA29A"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Critical", Tag ="Critical", ColorCode = "#DD5550"});
    }
    
    /// <summary>
    /// Sets the task status based on the given status name, updates the task DTO, and triggers an update in the task service
    /// </summary>
    /// <param name="status"></param>
    public void SetStatus(string? status)
    {
        Status = StatusList.First(s => s.Name == status);
        _taskDto.Status = Status.Name.Replace(" ","");
        _taskService.UpdateTask(_taskDto);
    }

    /// <summary>
    /// Sets the task priority based on the given priority name, updates the task DTO, and triggers an update in the task service
    /// </summary>
    /// <param name="priority"></param>
    public void SetPriority(string? priority)
    {
        Priority = PriorityList.First(s => s.Name == priority);
        _taskDto.Priority = Priority.Name.Replace(" ","");
        _taskService.UpdateTask(_taskDto);
    }

    /// <summary>
    /// Updates the task's start date and saves the changes, sets visibility on false
    /// </summary>
    public void EditStartDate(DateTime newDate)
    {
        _taskDto.StartDate = newDate;
        _taskService.UpdateTask(_taskDto);
        IsEditingStartDate = false;
    }
    
    /// <summary>
    /// Updates the task's due date and triggers an update in the task service
    /// </summary>
    /// <param name="newDate"></param>
    public void EditDueDate(DateTime newDate)
    {
        _taskDto.DueDate = newDate;
        _taskService.UpdateTask(_taskDto);
        IsEditingDueDate = false;
    }
    
    /// <summary>
    /// Sets the flags for editing start and due dates to false when the date picker is closed
    /// </summary>
    public void CloseDatePicker()
    {
        IsEditingStartDate = false;
        IsEditingDueDate = false;
    }
    
    /// <summary>
    /// Saves the task description if it has changed and updates the task
    /// </summary>
    private void SaveDescription()
    {
        if (_taskDto.Description != Description)
        {
            _taskDto.Description = Description;
            _taskService.UpdateTask(_taskDto);
        }
    }

    /// <summary>
    /// Toggles the editing state of the description by switching the IsEditingDescription flag
    /// </summary>
    public void ToggleDescriptionButton()
    {
        IsEditingDescription = !IsEditingDescription;   
    }
    
    /// <summary>
    /// Assigns a user to this task (not yet fully implemented)
    /// </summary>
    private void AddTaskUser()
    {
        Console.WriteLine($"Adding task user");
        _taskService.AddUserToTask(Username, _taskDto.TaskId);
    }
}