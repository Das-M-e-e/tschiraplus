using System;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using Services;
using Services.DatabaseServices;
using Services.ProjectServices;
using Services.Repositories;
using Services.TaskServices;
using UI.Views;

namespace UI.ViewModels;

public class MainViewModel : ViewModelBase
{
    // Services
    private readonly DatabaseService _dbService;
    private readonly ApplicationState _appState;
    private readonly SyncService _syncService;
    private readonly WrapperViewModel _wrapper;
    
    // Bindings
    private bool _isPaneOpen;
    public bool IsPaneOpen
    {
        get => _isPaneOpen;
        set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
    }

    private string _toggleButtonSymbol;

    public string ToggleButtonSymbol
    {
        get => _toggleButtonSymbol;
        set => this.RaiseAndSetIfChanged(ref _toggleButtonSymbol, value);
    }

    private UserControl _currentContent;
    public UserControl CurrentContent
    {
        get => _currentContent;
        set => this.RaiseAndSetIfChanged(ref _currentContent, value);
    }
    
    public UserControl PaneContent { get; set; }
    
    // Commands
    public ICommand ToggleSidebarCommand { get; }

    public MainViewModel(DatabaseService dbService, ApplicationState appState, SyncService syncService, WrapperViewModel wrapper)
    {
        _dbService = dbService;
        _appState = appState;
        _syncService = syncService;
        _wrapper = wrapper;
        
        IsPaneOpen = true;
        ToggleButtonSymbol = "<<";
        ToggleSidebarCommand = new RelayCommand(ToggleSidebar);
        PaneContent = new MainMenuView
        {
            DataContext = new MainMenuViewModel(
                this,
                new ProjectService(
                    new ProjectRepository(_dbService.GetDatabase(), new RemoteDatabaseService()),
                    new ProjectUserRepository(_dbService.GetDatabase(), new RemoteDatabaseService()),
                    _appState)
                )
        };

        OpenProjectList();
    }

    /// <summary>
    /// Opens or CLoses the sidebar
    /// </summary>
    private void ToggleSidebar()
    {
        IsPaneOpen = !IsPaneOpen;
        ToggleButtonSymbol = IsPaneOpen ? "<<" : ">>";
    }

    /// <summary>
    /// Opens the ProjectListView
    /// </summary>
    public void OpenProjectList()
    {
        _syncService.StopTaskSync();
        CurrentContent = new ProjectListView
        {
            DataContext = new ProjectListViewModel(
                new ProjectService(
                    new ProjectRepository(_dbService.GetDatabase(), new RemoteDatabaseService()),
                    new ProjectUserRepository(_dbService.GetDatabase(), new RemoteDatabaseService()),
                    _appState),
                this,
                _appState)
        };
    }

    /// <summary>
    /// Opens a certain project by id
    /// </summary>
    /// <param name="projectId"></param>
    public void OpenProject(Guid projectId)
    {
        _syncService.StartTaskSync(projectId);
        
        var taskRepository = new TaskRepository(_dbService.GetDatabase(), new RemoteDatabaseService())
        {
            ProjectId = projectId
        };
        var taskSortingManager = new TaskSortingManager();
        var userInputParser = new UserInputParser();
        var taskService = new TaskService(taskRepository, taskSortingManager, _appState, userInputParser);

        var mainTabViewModel = new MainTabViewModel(taskService, _appState);

        CurrentContent = new MainTabView { DataContext = mainTabViewModel };
        
        _appState.CurrentProjectId = projectId;
    }

    /// <summary>
    /// Opens the UserProfileView
    /// </summary>
    public void OpenUserDetails()
    {
        _syncService.StopTaskSync();
        CurrentContent = new UserProfileView
        {
            DataContext = new UserProfileViewModel(_wrapper)
        };
    }

    /// <summary>
    /// Opens the SettingsView
    /// </summary>
    public void OpenSettings()
    {
        _syncService.StopTaskSync();
        CurrentContent = new SettingsView
        {
            DataContext = new SettingsViewModel()
        };
    }
}