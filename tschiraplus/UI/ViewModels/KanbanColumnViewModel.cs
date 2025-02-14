using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace UI.ViewModels;

public class KanbanColumnViewModel
{
    // Services
    private readonly TaskListViewModel _taskListViewModel;
    
    // Bindings
    public string Title { get; }
    public string Status { get; }
    public string BackgroundColor { get; }
    public string TaskBackgroundColor { get; }
    public ObservableCollection<TaskViewModel> Tasks { get; } = [];
    
    // Commands
    public ICommand OpenTaskCreationCommand { get; }
    
    public KanbanColumnViewModel(string title, string status, string backgroundColor, string taskBackgroundColor, TaskListViewModel taskListViewModel)
    {
        Title = title;
        Status = status;
        BackgroundColor = backgroundColor;
        TaskBackgroundColor = taskBackgroundColor;
        
        _taskListViewModel = taskListViewModel;

        OpenTaskCreationCommand = new RelayCommand<string>(OpenTaskCreation);
    }

    /// <summary>
    /// Opens the TaskCreationView in a flyout
    /// </summary>
    /// <param name="status"></param>
    private void OpenTaskCreation(string? status)
    {
        _taskListViewModel.OpenTaskCreationCommand.Execute(status);
    }
}