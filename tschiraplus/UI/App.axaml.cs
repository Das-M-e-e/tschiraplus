using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Services;
using Services.Repositories;
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
            var dbService = new DatabaseService("Data Source=localDatabase.db");
            dbService.InitializeDatabase();
            var taskRepository = new TaskRepository(dbService.GetDatabase());
            var mainTabViewModel = new MainTabViewModel(taskRepository);
            
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainTabView
            {
                DataContext = mainTabViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}