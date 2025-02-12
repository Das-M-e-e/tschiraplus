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
using Services.UserServices;
using UI.Views;

namespace UI.ViewModels;

public class MainViewModel : ViewModelBase
{
    // Services
    private readonly DatabaseService _dbService;
    private readonly IAuthService _authService;
    private readonly ApplicationState _appState;
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

    public MainViewModel(DatabaseService dbService, IAuthService authService, ApplicationState appState, WrapperViewModel wrapper)
    {
        _dbService = dbService;
        _authService = authService;
        _appState = appState;
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
                    _appState.CurrentUser)
                )
        };

        OpenProjectList();
    }

    private void ToggleSidebar()
    {
        IsPaneOpen = !IsPaneOpen;
        ToggleButtonSymbol = IsPaneOpen ? "<<" : ">>";
    }

    public void OpenProjectList()
    {
        CurrentContent = new ProjectListView
        {
            DataContext = new ProjectListViewModel(
                new ProjectService(
                    new ProjectRepository(_dbService.GetDatabase(), new RemoteDatabaseService()),
                    new ProjectUserRepository(_dbService.GetDatabase(), new RemoteDatabaseService()),
                    _appState.CurrentUser),
                this,
                _appState)
        };
    }

    public void OpenProject(Guid projectId)
    {
        var taskRepository = new TaskRepository(_dbService.GetDatabase(), new RemoteDatabaseService())
        {
            ProjectId = projectId
        };
        var taskSortingManager = new TaskSortingManager();
        var userInputParser = new UserInputParser();
        var taskService = new TaskService(taskRepository, taskSortingManager, _appState, userInputParser);

        var mainTabViewModel = new MainTabViewModel(taskService, projectId, _appState);

        CurrentContent = new MainTabView { DataContext = mainTabViewModel };
        
        _appState.CurrentProjectId = projectId;
    }

    public void OpenUserDetails()
    {
        CurrentContent = new UserProfileView();
    }

    public void OpenSettings()
    {
        CurrentContent = new SettingsView
        {
            DataContext = new SettingsViewModel()
        };
    }
}