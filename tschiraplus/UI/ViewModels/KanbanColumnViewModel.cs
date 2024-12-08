using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.TaskServices;

namespace UI.ViewModels;

public class KanbanColumnViewModel
{
    private readonly TaskService _taskService;
    private readonly TaskListViewModel _taskListViewModel;
    
    public string Title { get; }
    public string Status { get; }
    public string BackgroundColor { get; }
    public string TaskBackgroundColor { get; }

    public ObservableCollection<TaskViewModel> Tasks { get; } = new();
    
    public ICommand AddRandomTaskCommand { get; }

    public KanbanColumnViewModel(string title, string status, string backgroundColor, string taskBackgroundColor,
        TaskService taskService, TaskListViewModel taskListViewModel) //Konstruktor
    {
        Title = title;
        Status = status;
        BackgroundColor = backgroundColor;
        TaskBackgroundColor = taskBackgroundColor;
        _taskService = taskService;
        _taskListViewModel = taskListViewModel;

        AddRandomTaskCommand = new RelayCommand(AddRandomTask);
    }

    private void AddRandomTask() //Stellt eine zuffälige Task (Testzwecke)
    {
        _taskService.AddRandomTask(Status);
        _taskListViewModel.LoadTasks();
    }
}