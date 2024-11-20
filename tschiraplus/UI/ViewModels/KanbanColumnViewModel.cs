using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.Factories;
using Services.Repositories;

namespace UI.ViewModels;

public class KanbanColumnViewModel
{
    private readonly TaskRepository _taskRepository;
    private readonly TaskListViewModel _taskListViewModel;
    
    public string Title { get; }
    public string Status { get; }
    public string BackgroundColor { get; }
    public string TaskBackgroundColor { get; }

    public ObservableCollection<TaskViewModel> Tasks { get; } = new();
    
    public ICommand AddRandomTaskCommand { get; }

    public KanbanColumnViewModel(string title, string status, string backgroundColor, string taskBackgroundColor,
        TaskRepository taskRepository, TaskListViewModel taskListViewModel)
    {
        Title = title;
        Status = status;
        BackgroundColor = backgroundColor;
        TaskBackgroundColor = taskBackgroundColor;
        _taskRepository = taskRepository;
        _taskListViewModel = taskListViewModel;

        AddRandomTaskCommand = new RelayCommand(AddRandomTask);
    }

    private void AddRandomTask()
    {
        var newTask = TaskFactory.CreateRandomTask(Status);
        _taskRepository.AddTask(newTask);

        var taskViewModel = new TaskViewModel(newTask, _taskListViewModel);
        
        Tasks.Add(taskViewModel);
        _taskListViewModel.Tasks.Add(taskViewModel);
    }
}