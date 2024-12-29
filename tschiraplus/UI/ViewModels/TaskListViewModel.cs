using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services.DTOs;
using Services.TaskServices;

namespace UI.ViewModels;


public class TaskListViewModel : ViewModelBase, IActivatableViewModel
{
    // Services
    public ViewModelActivator Activator { get; }
    private readonly ITaskService _taskService;
    
    // Bindings
    public ObservableCollection<TaskViewModel> Tasks { get; } = new();
    public ObservableCollection<KanbanColumnViewModel> KanbanColumns { get; } = new();
    private List<TaskDto> AllTasks { get; set; } = new();
    
    // Commands
    public ICommand AddRandomTaskCommand { get; }
    public ICommand SortTasksByTitleCommand { get; }
    public ICommand FilterTasksByStatusCommand { get; }

    public TaskListViewModel(ITaskService taskService)
    {
        _taskService = taskService;

        AddRandomTaskCommand = new RelayCommand(AddRandomTask);
        SortTasksByTitleCommand = new RelayCommand(SortTasksByTitle);
        FilterTasksByStatusCommand = new RelayCommand(FilterTasksByStatus);
        
        InitializeKanbanColumns();

        Activator = new ViewModelActivator();
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadTasks();
        });
    }

    /// <summary>
    /// Fills the KanbanColumns list with five initial columns
    /// </summary>
    private void InitializeKanbanColumns()
    {
        KanbanColumns.Add(new KanbanColumnViewModel("Backlog", "Backlog", "LightSteelBlue", "#ECF3FF", _taskService, this));
        KanbanColumns.Add(new KanbanColumnViewModel("Ready", "Ready", "MistyRose", "#FFF9F8", _taskService, this));
        KanbanColumns.Add(new KanbanColumnViewModel("In Progress", "InProgress", "#FFF5CD", "#FFFCF0", _taskService, this));
        KanbanColumns.Add(new KanbanColumnViewModel("In Review", "InReview", "Bisque", "#FFFAF4", _taskService, this));
        KanbanColumns.Add(new KanbanColumnViewModel("Done", "Done", "#B7DAA8", "#E6FFF1", _taskService, this));
    }

    /// <summary>
    /// Loads all tasks from the database
    /// </summary>
    public void LoadTasks()
    {
        AllTasks = _taskService.GetAllTasks();
        UpdateTaskList(AllTasks);
    }

    /// <summary>
    /// Updates the Tasks list using a given list of TaskDtos
    /// </summary>
    /// <param name="taskDtos"></param>
    private void UpdateTaskList(IEnumerable<TaskDto> taskDtos)
    {
        Tasks.Clear();
        foreach (var col in KanbanColumns)
        {
            col.Tasks.Clear();
        }
        
        foreach (var task in taskDtos)
        {
            Tasks.Add(new TaskViewModel(task, this));
            var column = KanbanColumns.FirstOrDefault(c => c.Status == task.Status);
            column?.Tasks.Add(new TaskViewModel(task, this));
        }
    }

    // Todo: Remove when custom task creation is implemented @Das_M_e_e_
    private void AddRandomTask()
    {
        _taskService.AddRandomTask("Backlog");
        LoadTasks();
    }

    /// <summary>
    /// Uses the _taskService to delete a task
    /// </summary>
    /// <param name="task"></param>
    public void DeleteTask(TaskViewModel task)
    {
        _taskService.DeleteTask(task.TaskId);
        LoadTasks();
    }

    // Todo: Remove when generic sorting/filtering is implemented @Das_M_e_e_
    private void SortTasksByTitle()
    {
        var taskDtos = Tasks.Select(task => new TaskDto
        {
            TaskId = task.TaskId,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreationDate = task.CreationDate
        }).ToList();
        
        var sortedTaskDtos = _taskService.SortTasksByTitle(taskDtos);
        UpdateTaskList(sortedTaskDtos);
    }

    // Todo: Remove when generic sorting/filtering is implemented @Das_M_e_e_
    private void FilterTasksByStatus()
    {
        var filteredTasks = _taskService.FilterTasksByStatus(AllTasks, "Backlog");
        UpdateTaskList(filteredTasks);
    }
}