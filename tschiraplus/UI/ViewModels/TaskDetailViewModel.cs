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
    private readonly MainTabViewModel _mainTabViewModel;
    
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

    public ObservableCollection<SelectionItemViewModel> StatusList { get; set; } = [];
    
    // Commands
    
    public ICommand StartEditingTitleCommand { get; set; }
    public ICommand SaveTitleCommand { get; set; }
    public ICommand CloseTabCommand { get; set; }
    
    
    public TaskDetailViewModel(ITaskService taskService, Guid taskId, MainTabViewModel mainTabViewModel)
    {
        _taskService = taskService;
        _mainTabViewModel = mainTabViewModel;
        LoadStatusList();
        LoadTask(taskId);
        
        StartEditingTitleCommand = new RelayCommand(StartEditingTitle);
        SaveTitleCommand = new RelayCommand(SaveTitle);
        CloseTabCommand = new RelayCommand(CloseTab);

    }

    /// <summary>
    /// Load the Task that has the given id
    /// </summary>
    /// <param name="taskId"></param>
    private void LoadTask(Guid taskId)
    {
        _taskDto = _taskService.GetTaskById(taskId);
        Title = _taskDto.Title;
        Status = StatusList.First(s => (string)s.Tag! == _taskDto.Status);
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

    public void SetStatus(string? status)
    {
        Status = StatusList.First(s => s.Name == status);
        _taskDto.Status = Status.Name.Replace(" ","");
        _taskService.UpdateTask(_taskDto);
    }

    private void CloseTab()
    {
        _mainTabViewModel.CloseCurrentTab();
    }
    
}