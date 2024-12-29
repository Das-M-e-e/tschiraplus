using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using PetaPoco;
using Services;
using Services.API;
using Services.DatabaseServices;
using Services.Repositories;
using Services.UserServices;
using UI.ViewModels;
using UI.Views;

namespace UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            var appState = new ApplicationState();
            var dbService = new DatabaseService("localDatabase.db");
            dbService.InitializeDatabase();

            var connectivityService = new ConnectivityService();
            var isHostReachable = connectivityService.IsHostReachable();
            
            if (isHostReachable)
            {
                Console.WriteLine("Host available.");
                appState.IsOnline = true;
            }
            else
            {
                Console.WriteLine("Host unavailable. App will operate in offline mode.");
                appState.IsOnline = false;
            }

            var wrapperViewModel = new WrapperViewModel(appState);

            desktop.MainWindow = new WrapperView
            {
                DataContext = wrapperViewModel
            };

            _ = PerformTokenCheckAsync(appState, wrapperViewModel);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async Task PerformTokenCheckAsync(ApplicationState appState, WrapperViewModel wrapperViewModel)
    {
        try
        {
            var savedToken = TokenStorageService.LoadToken();
            if (savedToken != null)
            {
                var userService = new UserService(
                    new UserRepository(new DatabaseService("localDatabase.db").GetDatabase(),
                        new RemoteDatabaseService()),
                    new AuthService(new RemoteDatabaseService()),
                    appState);
                
                var isAuthenticated = await userService.AuthenticateWithTokenAsync(savedToken);

                if (isAuthenticated)
                {
                    Console.WriteLine("User logged in with saved token.");
                    wrapperViewModel.NavigateToMainMenu();
                }
                else
                {
                    Console.WriteLine("Token is expired");
                }
            }
            else
            {
                Console.WriteLine("No saved token");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}