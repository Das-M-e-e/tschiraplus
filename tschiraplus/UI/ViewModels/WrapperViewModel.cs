using System;
using System.Threading.Tasks;
using ReactiveUI;
using Services;
using Services.DatabaseServices;
using Services.Repositories;
using Services.UserServices;
using UI.Views;

namespace UI.ViewModels;

public class WrapperViewModel : ViewModelBase
{
    // Services
    private readonly ApplicationState _appState;
    private readonly SyncService _syncService;

    // Bindings
    private object _currentView;
    public object CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public WrapperViewModel(ApplicationState appState)
    {
        _appState = appState;
        var db = new DatabaseService("localDatabase.db").GetDatabase();
        _syncService = new SyncService(
            new RemoteDatabaseService(),
            new UserRepository(db, new RemoteDatabaseService()),
            new ProjectRepository(db, new RemoteDatabaseService()),
            new ProjectUserRepository(db, new RemoteDatabaseService()),
            new TaskRepository(db, new RemoteDatabaseService()));

        if (_appState.CurrentUser != null)
        {
            NavigateToMainMenu();
        }
        else
        {
            NavigateToLogin();
        }
    }

    /// <summary>
    /// Sets the current view (content) of the wrapper to be the MainMenuView
    /// </summary>
    public async Task NavigateToMainMenu()
    {
        await Sync();
        
        _syncService.StartProjectSync(_appState.CurrentUser!.UserId);

        CurrentView = new MainView
        {
            DataContext = new MainViewModel(
                new DatabaseService("localDatabase.db"),
                _appState,
                _syncService)
        };
    }

    /// <summary>
    /// Sets the current view (content) of the wrapper to be the LoginView
    /// </summary>
    public void NavigateToLogin()
    {
        _syncService.StopProjectSync();
        
        CurrentView = new LoginView
        {
            DataContext = new LoginViewModel(this, _appState)
        };
    }

    /// <summary>
    /// Sets the current view (content) of the wrapper to be the RegisterView
    /// </summary>
    public void NavigateToRegister()
    {
        CurrentView = new RegisterView
        {
            DataContext = new RegisterViewModel(
                new UserService(
                    new UserRepository(new DatabaseService("localDatabase.db").GetDatabase(), new RemoteDatabaseService()),
                    new AuthService(new RemoteDatabaseService()),
                    _appState),
                this)
        };
    }

    /// <summary>
    /// Starts the synchronization process
    /// </summary>
    private async Task Sync()
    {
        try
        {
            await _syncService.SyncProjectsAsync(_appState.CurrentUser.UserId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Synchronization failed: {e.Message}");
        }
    }
}