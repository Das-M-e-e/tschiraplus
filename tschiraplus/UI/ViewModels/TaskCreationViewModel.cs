using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services.DTOs;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    // Services
    private readonly ITaskService _taskService;
    private readonly TaskListViewModel _taskListViewModel;

    // Bindings
    private string _title;
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }
    private string _description;
    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    private SelectionItemViewModel _priority;
    public SelectionItemViewModel Priority
    {
        get => _priority;
        set => this.RaiseAndSetIfChanged(ref _priority, value);
    }
    private string _titleErrorMessage;
    public string TitleErrorMessage
    {
        get => _titleErrorMessage;
        set => this.RaiseAndSetIfChanged(ref _titleErrorMessage, value);
    }
    private bool _isTitleErrorMessageVisible;
    public bool IsTitleErrorMessageVisible
    {
        get => _isTitleErrorMessageVisible;
        set => this.RaiseAndSetIfChanged(ref _isTitleErrorMessageVisible, value);
    }
    public string InitialStatus { get; set; }

    public ObservableCollection<SelectionItemViewModel> PriorityList { get; set; } = [];
    
    public TaskCreationViewModel(ITaskService taskService, TaskListViewModel taskListViewModel)
    {
        _taskService = taskService;
        _taskListViewModel = taskListViewModel;

        Priority = new SelectionItemViewModel { Name = "Choose...", ColorCode = "#323232" };
        LoadPriorityList();
    }

    private void LoadPriorityList()
    {
        PriorityList.Add(new SelectionItemViewModel{Name = "Low", Tag ="Low", ColorCode = "#95D8A1"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Medium", Tag ="Medium", ColorCode = "#8FCDF3"});
        PriorityList.Add(new SelectionItemViewModel{Name = "High", Tag ="High", ColorCode = "#EBA29A"});
        PriorityList.Add(new SelectionItemViewModel{Name = "Critical", Tag ="Critical", ColorCode = "#DD5550"});
    }

    /// <summary>
    /// Uses the _taskService to create a new task
    /// </summary>
    public bool CreateTask()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            TitleErrorMessage = "Title can't be empty!";
            IsTitleErrorMessageVisible = true;
            return false;
        }

        var task = new TaskDto
        {
            Title = Title,
            Description = Description,
            Priority = Priority.Tag?.ToString() ?? "NotSet",
            Status = InitialStatus
        };
        _taskService.CreateTask(task);
        _taskListViewModel.LoadTasks();

        IsTitleErrorMessageVisible = false;
        return true;
    }
}