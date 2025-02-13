using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class ProjectListView : ReactiveUserControl<ProjectListViewModel>
{
    public ProjectListView()
    {
        InitializeComponent();
    }

    public void OnEditButtonPressed(object sender, RoutedEventArgs args)
    {
        var grid = RootGrid;
        var flyout = grid.ContextFlyout;
        flyout?.ShowAt(grid);
    }

    public void OnCreateProjectButtonPressed(object? sender, RoutedEventArgs args)
    {
        var grid = RootGrid;
        var flyout = RootGrid.ContextFlyout;
        flyout?.ShowAt(grid);
    }

    public void OnCloseButtonPressed()
    {
        RootGrid.ContextFlyout?.Hide();
    }
}