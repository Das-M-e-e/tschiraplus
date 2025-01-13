using System;
using System.Collections.ObjectModel;
using Avalonia.Collections;
using ReactiveUI;
using Services.TaskServices;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase
{
    // Services
    private readonly ITaskService _taskService;
    // Bindings
    public ObservableCollection<TabItemViewModel> Tabs { get; }
    
    public MainTabViewModel(ITaskService taskService)
    {
        _taskService = taskService;
        var taskListViewModel = new TaskListViewModel(_taskService);
        
        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new("KanbanView", new KanbanView { DataContext = taskListViewModel }),
            new("TaskListView", new TaskListView{ DataContext = taskListViewModel })
        };
    }
}