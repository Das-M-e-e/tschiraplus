﻿using System;
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
    private readonly ISyncService _syncService;

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
        _syncService = new SyncService(
            new ProjectUserRepository(new DatabaseService("localDatabase.db").GetDatabase(), new RemoteDatabaseService()),
            new ProjectRepository(new DatabaseService("localDatabase.db").GetDatabase(), new RemoteDatabaseService()),
            new UserRepository(new DatabaseService("localDatabase.db").GetDatabase(), new RemoteDatabaseService()), 
            new TaskRepository(new DatabaseService("localDatabase.db").GetDatabase(), new RemoteDatabaseService()),
            _appState);

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

        CurrentView = new MainView
        {
            DataContext = new MainViewModel(
                new DatabaseService("localDatabase.db"),
                new AuthService(new RemoteDatabaseService()),
                _appState,
                this)
        };
    }

    /// <summary>
    /// Sets the current view (content) of the wrapper to be the LoginView
    /// </summary>
    public void NavigateToLogin()
    {
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
            await _syncService.SyncAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Synchronization failed: {e.Message}");
        }
    }
}