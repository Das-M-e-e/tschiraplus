﻿using System.Collections.ObjectModel;
using Services;
using Services.DatabaseServices;
using Services.Repositories;
using Services.TaskServices;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase
{
    private TaskService _taskService;
    
    public ObservableCollection<TabItemViewModel> Tabs { get; }

    public MainTabViewModel(TaskService taskService)
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