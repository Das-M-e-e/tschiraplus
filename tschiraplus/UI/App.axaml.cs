using System;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
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
            
            var userService = new UserService(
                new UserRepository(
                    dbService.GetDatabase()),
                new AuthService(
                    new RemoteDatabaseService()
                    )
                );
            userService.AddUserIfNoneExists();
            appState.CurrentUser = userService.GetSystemUser();

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

            desktop.MainWindow = new MainMenuView
            {
                DataContext = new MainMenuViewModel(dbService, appState)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}