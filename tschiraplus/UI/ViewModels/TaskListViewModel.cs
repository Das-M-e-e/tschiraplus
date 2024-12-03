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
    public ViewModelActivator Activator { get; }

    private readonly TaskService _taskService;
    // Lists
    public ObservableCollection<TaskViewModel> Tasks { get; } = new();
    public ObservableCollection<KanbanColumnViewModel> KanbanColumns { get; } = new();

    private List<TaskDto> AllTasks { get; set; } = new();
    // Commands
    public ICommand AddRandomTaskCommand { get; }
    public ICommand SortTasksByTitleCommand { get; }
    public ICommand FilterTasksByStatusCommand { get; }

    public TaskListViewModel(TaskService taskService) //Konstruktor
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

    private void InitializeKanbanColumns() //Stellt Spalten aus Kanban bereit
    {
        KanbanColumns.Add(new KanbanColumnViewModel("Backlog", "Backlog", "LightSteelBlue", "#ECF3FF", _taskService, this));
        KanbanColumns.Add(new KanbanColumnViewModel("Ready", "Ready", "MistyRose", "#FFF9F8", _taskService, this));
        KanbanColumns.Add(new KanbanColumnViewModel("In Progress", "InProgress", "#FFF5CD", "#FFFCF0", _taskService, this));
        KanbanColumns.Add(new KanbanColumnViewModel("In Review", "InReview", "Bisque", "#FFFAF4", _taskService, this));
        KanbanColumns.Add(new KanbanColumnViewModel("Done", "Done", "#B7DAA8", "#E6FFF1", _taskService, this));
    }

    public void LoadTasks() //Aktualiesiert und zeigt die Liste der Aufgaben
    {
        AllTasks = _taskService.GetAllTasks();
        UpdateTaskList(AllTasks);
    }

    private void UpdateTaskList(IEnumerable<TaskDto> taskDtos) //Aktualisiert die Zeilen anhand einer Liste von Dto's
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

    private void AddRandomTask() //Erstellt eine zufällige Aufgabe (Testzwecke)
    {
        _taskService.AddRandomTask("Backlog");
        LoadTasks();
    }

    public void DeleteTask(TaskViewModel task) //Löscht eine Aufgabe per ID
    {
        _taskService.DeleteTask(task.TaskId);
        LoadTasks();
    }

    private void SortTasksByTitle() //Arangiert die Aufgaben anhand ihres Titels (alphabetisch?)
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

    private void FilterTasksByStatus() //Zeigt nur Aufgaben eines bestimmten Status
    {
        var filteredTasks = _taskService.FilterTasksByStatus(AllTasks, "Backlog");
        UpdateTaskList(filteredTasks);
    }
}