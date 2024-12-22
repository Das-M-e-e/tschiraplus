using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Services.DatabaseServices;
using Services.ProjectServices;
using Services.Repositories;
using UI.Views;

namespace UI.ViewModels;

public class MainMenuViewModel : ObservableObject
{
    private readonly DatabaseService _dbService;
    
    public ObservableCollection<TabItemViewModel> Tabs { get; }
    
    public ICommand OpenProjectCommand { get; }

    private TabItemViewModel _currentProjectTab;

    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }
    
    public MainMenuViewModel(DatabaseService dbService)
    {
        _dbService = dbService;

        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new("Projects", new ProjectListView{DataContext = new ProjectListViewModel(new ProjectService(_dbService, new ProjectRepository(_dbService.GetDatabase())), this)}),
            new("Create Project Test", new CreateNewProjectView{DataContext = new CreateNewProjectViewModel(new ProjectService(_dbService, new ProjectRepository(_dbService.GetDatabase())))})
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

        var databaseService = new DatabaseService(DatabasePathHelper.GetDatabasePath($"project_{projectId}.db"));
        var taskRepository = new TaskRepository(databaseService.GetDatabase());
        var mainTabViewModel = new MainTabViewModel(taskRepository);

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