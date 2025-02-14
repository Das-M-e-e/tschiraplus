using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Services;
using Avalonia.Controls;
using ReactiveUI;
using Services.TaskServices;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase
{
    // Services
    private readonly ITaskService _taskService;
    private readonly TaskListViewModel _taskListViewModel;
    private readonly ApplicationState _appState;
    
    // Bindings
    public ObservableCollection<TabItemViewModel> Tabs { get; }
    
    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
    }
    
    private UserControl _selectedFlyout;
    public UserControl SelectedFlyout
    {
        get => _selectedFlyout;
        set => this.RaiseAndSetIfChanged(ref _selectedFlyout, value);
    }
    
    public bool TaskCheckBoxClicked { get; set; }

    public MainTabViewModel(ITaskService taskService, ApplicationState appState)
    {
        _taskService = taskService;
        _appState = appState;
        
        _taskListViewModel = new TaskListViewModel(_taskService, this, _appState);
        
        Tabs =
        [
            new TabItemViewModel("KanbanView", new KanbanView { DataContext = _taskListViewModel }),
            new TabItemViewModel("TaskListView", new TaskListView { DataContext = _taskListViewModel })
        ];
    }

    /// <summary>
    /// Navigates to the TaskCreationView
    /// </summary>
    public void CreateNewTask(string? status)
    {
        var view = new TaskCreationView
        {
            DataContext = new TaskCreationViewModel(_taskService, _taskListViewModel)
            {
                InitialStatus = status ?? "Backlog"
            }
        };
        SetFlyoutContent(view);
    }

    /// <summary>
    /// Opens a certain tasks TaskDetailView in a flyout
    /// </summary>
    /// <param name="taskId"></param>
    public void ShowTaskDetails(Guid taskId)
    {
        var view = new TaskDetailView
        {
            DataContext = new TaskDetailViewModel(
                _taskService,
                taskId)
        };
        SetFlyoutContent(view);
    }

    /// <summary>
    /// Sets the content of the flyout
    /// </summary>
    /// <param name="content"></param>
    private void SetFlyoutContent(UserControl content)
    {
        SelectedFlyout = content;
    }

    /// <summary>
    /// Updates the task list by reloading the tasks from the view model
    /// </summary>
    public async Task UpdateTaskList()
    {
        await _taskListViewModel.LoadTasks();
    }
}