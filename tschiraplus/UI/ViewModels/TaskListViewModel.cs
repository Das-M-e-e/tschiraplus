using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services.Repositories;
using Services.TaskProcessing;
using TaskFactory = Services.Factories.TaskFactory;

namespace UI.ViewModels;


public class TaskListViewModel : ViewModelBase, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }

    private readonly TaskRepository _taskRepository;
    private readonly TaskSortingManager _taskSortingManager;
    public ObservableCollection<TaskViewModel> Tasks { get; } = new();
    public ObservableCollection<KanbanColumnViewModel> KanbanColumns { get; } = new();
    public ICommand AddRandomTaskCommand { get; }
    public ICommand SortTasksByTitleCommand { get; }
    public ICommand SortTasksByCreationDateCommand { get; }

    public TaskListViewModel(TaskRepository taskRepository, TaskSortingManager taskSortingManager)
    {
        _taskRepository = taskRepository;
        _taskSortingManager = taskSortingManager;

        AddRandomTaskCommand = new RelayCommand(AddRandomTask);
        SortTasksByTitleCommand = new RelayCommand(SortTasksByTitle);
        SortTasksByCreationDateCommand = new RelayCommand(SortTasksByCreationDate);
        
        InitializeKanbanColumns();
        //LoadTasks();

        Activator = new ViewModelActivator();
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadTasks();
        });
    }

    private void InitializeKanbanColumns()
    {
        KanbanColumns.Add(new KanbanColumnViewModel("Backlog", "Backlog", "LightSteelBlue", "#ECF3FF", _taskRepository, this));
        KanbanColumns.Add(new KanbanColumnViewModel("Ready", "Ready", "MistyRose", "#FFF9F8", _taskRepository, this));
        KanbanColumns.Add(new KanbanColumnViewModel("In Progress", "InProgress", "#FFF5CD", "#FFFCF0", _taskRepository, this));
        KanbanColumns.Add(new KanbanColumnViewModel("In Review", "InReview", "Bisque", "#FFFAF4", _taskRepository, this));
        KanbanColumns.Add(new KanbanColumnViewModel("Done", "Done", "#B7DAA8", "#E6FFF1", _taskRepository, this));
    }

    private void LoadTasks()
    {
        // empty tasks lists, so no task is shown multiple times
        Tasks.Clear();
        foreach (var column in KanbanColumns)
        {
            column.Tasks.Clear();
        }
        
        // load all tasks
        var allTasks = _taskRepository.GetAllTasks();
        
        // put the tasks in the tasks lists
        foreach (var task in allTasks)
        {
            Tasks.Add(new TaskViewModel(task, this));
            var column = KanbanColumns.FirstOrDefault(c => c.Status == task.Status?.ToString());
            column?.Tasks.Add(new TaskViewModel(task, this));
        }
    }

    private void AddRandomTask()
    {
        var newTask = TaskFactory.CreateRandomTask("Backlog");

        _taskRepository.AddTask(newTask);

        var newTaskViewModel = new TaskViewModel(newTask, this);
        Tasks.Add(newTaskViewModel);
        var column = KanbanColumns.FirstOrDefault(c => c.Status == newTask.Status.ToString());
        column?.Tasks.Add(newTaskViewModel);
    }

    public void DeleteTask(TaskViewModel task)
    {
        Tasks.Remove(task);
        _taskRepository.DeleteTask(task.TaskId);

        var column = KanbanColumns.FirstOrDefault(c => c.Status == task.Status);
        column?.Tasks.Remove(task);
    }

    private void SortTasksByTitle()
    {
        var sortedTasks = _taskSortingManager.SortBySingleAttribute(Tasks, task => task.Title);
        Tasks.Clear();
        foreach (var task in sortedTasks)
        {
            Tasks.Add(task);
        }
    }

    private void SortTasksByCreationDate()
    {
        var sortedTasks = _taskSortingManager.SortBySingleAttribute(Tasks, task => task.CreationDate);
        Tasks.Clear();
        foreach (var task in sortedTasks)
        {
            Tasks.Add(task);
        }
    }
}