using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Collections;
using ReactiveUI;
using Services.TaskServices;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ObservableObject
{
    // Services
    private readonly ITaskService _taskService;
    private readonly Guid _projectId;
    
    // Bindings
    public ObservableCollection<TabItemViewModel> Tabs { get; }
    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }
    
    private TabItemViewModel? _currentTab;

    public MainTabViewModel(ITaskService taskService, Guid projectId)
    {
        _taskService = taskService;
        _projectId = projectId;
        var taskListViewModel = new TaskListViewModel(_taskService, this);
        
        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new("KanbanView", new KanbanView { DataContext = taskListViewModel }),
            new("TaskListView", new TaskListView{ DataContext = taskListViewModel })
        };
    }

    /// <summary>
    /// Navigates to a given tab
    /// </summary>
    /// <param name="tab"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void NavigateToTab(TabItemViewModel tab)
    {
        var tabIndex = Tabs.IndexOf(tab);
        if (tabIndex >= 0)
        {
            SelectedTabIndex = tabIndex;
        }
        else
        {
            throw new InvalidOperationException("Tab not found in Tabs collection.");
        }
    }

    /// <summary>
    /// Navigates to the TaskCreationView
    /// </summary>
    public void CreateNewTask(string? status)
    {
        _currentTab = new TabItemViewModel(
            "New Task",
            new TaskCreationView
            {
                DataContext = new TaskCreationViewModel(_taskService, this)
                {
                    InitialStatus = status ?? "Backlog"
                }
            })
        {
            CanClose = true
        };
        Tabs.Add(_currentTab);
        NavigateToTab(_currentTab);
    }

    public void CloseCurrentTab()
    {
        if (_currentTab!.CanClose)
        {
            Tabs.Remove(_currentTab);
        }
    }
}