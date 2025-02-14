using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services;
using Services.DTOs;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskListViewModel : ViewModelBase, IActivatableViewModel
{
    // Services
    public ViewModelActivator Activator { get; }
    private readonly ITaskService _taskService;
    private readonly MainTabViewModel _mainTabViewModel;
    private readonly ApplicationState _appState;
    
    // Bindings
    public ObservableCollection<TaskViewModel> Tasks { get; } = [];
    public ObservableCollection<KanbanColumnViewModel> KanbanColumns { get; } = [];
    private List<TaskDto> AllTasks { get; set; } = [];
    public string UserInput { get; set; }
    
    // Commands
    public ICommand OpenTaskCreationCommand { get; }
    public ICommand ManipulateTasksCommand { get; }

    public TaskListViewModel(ITaskService taskService, MainTabViewModel mainTabViewModel, ApplicationState appState)
    {
        _taskService = taskService;
        _mainTabViewModel = mainTabViewModel;
        _appState = appState;

        OpenTaskCreationCommand = new RelayCommand<string>(OpenTaskCreation);
        ManipulateTasksCommand = new RelayCommand(ManipulateTasks);
        
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
        KanbanColumns.Add(new KanbanColumnViewModel("Backlog", "Backlog", "LightSteelBlue", "#ECF3FF", this));
        KanbanColumns.Add(new KanbanColumnViewModel("Ready", "Ready", "MistyRose", "#FFF9F8", this));
        KanbanColumns.Add(new KanbanColumnViewModel("In Progress", "InProgress", "#FFF5CD", "#FFFCF0", this));
        KanbanColumns.Add(new KanbanColumnViewModel("In Review", "InReview", "Bisque", "#FFFAF4", this));
        KanbanColumns.Add(new KanbanColumnViewModel("Done", "Done", "#B7DAA8", "#E6FFF1", this));
    }

    /// <summary>
    /// Loads all tasks from the database
    /// </summary>
    public async Task LoadTasks()
    {
        AllTasks = await _taskService.GetAllTasks();
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

    /// <summary>
    /// Uses the _mainTabViewModel to navigate to the TaskCreationView
    /// </summary>
    private void OpenTaskCreation(string? status)
    {
        _mainTabViewModel.CreateNewTask(status);
    }

    /// <summary>
    /// Uses the _mainTabViewModel to navigate to the TaskDetailView
    /// </summary>
    /// <param name="taskId"></param>
    public void OpenTaskDetails(Guid taskId)
    {
        _mainTabViewModel.ShowTaskDetails(taskId);
    }

    /// <summary>
    /// Sets a tasks status to "Done"
    /// </summary>
    /// <param name="taskId"></param>
    public void ToggleTaskDone(Guid taskId)
    {
        var task = AllTasks.FirstOrDefault(t => t.TaskId == taskId);
        task!.Status = task.Status.Equals("Done") ? "Ready" : "Done";
        _taskService.UpdateTask(task);
    }
    
    /// <summary>
    /// Sorts or filters the tasks
    /// </summary>
    private void ManipulateTasks()
    {
        var manipulatedTask  = _taskService.ProcessUserInput(UserInput, AllTasks);
        UpdateTaskList(manipulatedTask);
    }
}