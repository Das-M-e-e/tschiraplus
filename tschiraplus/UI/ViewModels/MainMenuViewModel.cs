using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;
using Services.DatabaseServices;
using Services.ProjectServices;
using Services.Repositories;
using Services.TaskServices;
using Services.UserServices;
using UI.Views;
namespace UI.ViewModels;

public class MainMenuViewModel : ObservableObject
{
    // Services
    private readonly DatabaseService _dbService;
    private readonly ApplicationState _appState;
    private readonly IAuthService _authService;
    private readonly WrapperViewModel _wrapper;
    
    // Bindings
    public ObservableCollection<TabItemViewModel> Tabs { get; }
    
    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }
    
    // Commands
    public ICommand OpenProjectCommand { get; }
    public ICommand LogoutUserCommand { get; }

    private TabItemViewModel? _currentProjectTab;
    
    public MainMenuViewModel(DatabaseService dbService, ApplicationState appState, IAuthService authService, WrapperViewModel wrapper)
    {
        _dbService = dbService;
        _appState = appState;
        _authService = authService;
        _wrapper = wrapper;

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
        LogoutUserCommand = new AsyncRelayCommand(LogoutUser);
    }

    /// <summary>
    /// Opens the selected project in a new tab
    /// </summary>
    /// <param name="projectId"></param>
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
            if (Tabs.Contains(_currentProjectTab))
            {
                Tabs.Remove(_currentProjectTab);
            }
        }

        var taskRepository = new TaskRepository(_dbService.GetDatabase(), projectId);
        _appState.CurrentProjectId = projectId;
        var taskSortingManager = new TaskSortingManager(taskRepository, _appState);
        var userInputParser = new UserInputParser();
        var taskService = new TaskService(taskRepository, taskSortingManager, _appState, userInputParser);

        var mainTabViewModel = new MainTabViewModel(taskService);

        _currentProjectTab = new TabItemViewModel($"{projectId}", new MainTabView { DataContext = mainTabViewModel })
        {
            Tag = projectId
        };
        
        Tabs.Add(_currentProjectTab);
        NavigateToTab(_currentProjectTab);
    }
    /// <summary>
    /// Navigates to the selected tab
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
    /// Uses the _authService to log out a user
    /// </summary>
    private async Task LogoutUser()
    {
        await _authService.LogoutAsync();
        _wrapper.NavigateToLogin();
    }
}