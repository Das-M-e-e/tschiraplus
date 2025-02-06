using System;
using System.Collections.ObjectModel;
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
    private readonly Guid _projectId;
    private readonly ApplicationState _appState;
    
    // Bindings
    public ObservableCollection<TabItemViewModel> Tabs { get; }
    private TabItemViewModel? _currentTab;
    
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

    private TaskListViewModel _taskListViewModel;

    public MainTabViewModel(ITaskService taskService, Guid projectId, ApplicationState appState)
    {
        _taskService = taskService;
        _projectId = projectId;
        _appState = appState;
        
        _taskListViewModel = new TaskListViewModel(_taskService, this, _appState);
        
        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new("KanbanView", new KanbanView { DataContext = _taskListViewModel }),
            new("TaskListView", new TaskListView{ DataContext = _taskListViewModel })
        };
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

    private void SetFlyoutContent(UserControl content)
    {
        SelectedFlyout = content;
    }
}