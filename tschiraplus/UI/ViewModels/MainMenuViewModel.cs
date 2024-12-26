using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Services.DatabaseServices;
using Services.ProjectServices;
using Services.Repositories;
using Services.TaskServices;
using UI.Views;

namespace UI.ViewModels;

public class MainMenuViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    private readonly ApplicationState _appState;
    
    public ObservableCollection<TabItemViewModel> Tabs { get; }
    
    public ICommand OpenProjectCommand { get; }

    private TabItemViewModel? _currentProjectTab;

    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }
    
    public MainMenuViewModel(DatabaseService dbService, ApplicationState appState)
    {
        _dbService = dbService;
        _appState = appState;

        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new("Projects", new ProjectListView
            {
                DataContext = new ProjectListViewModel(
                    new ProjectService(
                        new ProjectRepository(
                            _dbService.GetDatabase(),
                            new RemoteDatabaseService()),
                        _appState.CurrentUser),
                    this,
                    _appState)
            })
        };

        OpenProjectCommand = new RelayCommand<Guid>(OpenProject);
    }

    private void OpenProject(Guid projectId)
    {
        if (_currentProjectTab is { Tag: Guid currentProjectId })
        {
            if (currentProjectId == projectId)
            {
                NavigateToTab(_currentProjectTab);
                return;
            }

            Tabs.Remove(_currentProjectTab);
        }

        var taskRepository = new TaskRepository(_dbService.GetDatabase(), projectId);
        _appState.CurrentProjectId = projectId;
        var taskService = new TaskService(taskRepository, new TaskSortingManager(), _appState);
        
        var mainTabViewModel = new MainTabViewModel(taskService);

        _currentProjectTab = new TabItemViewModel($"{projectId}", new MainTabView { DataContext = mainTabViewModel })
        {
            Tag = projectId
        };
        
        Tabs.Add(_currentProjectTab);
        NavigateToTab(_currentProjectTab);
    }

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
}