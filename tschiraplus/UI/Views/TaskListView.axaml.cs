using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class TaskListView : ReactiveUserControl<TaskListViewModel>
{
    public TaskListView()
    {
        InitializeComponent();
    }

    public void OnEditButtonPressed(object? sender, RoutedEventArgs args)
    {
        var grid = RootGrid;
        var flyout = grid.ContextFlyout;
        flyout?.ShowAt(grid);
    }

    public void OnCloseButtonPressed()
    {
        RootGrid.ContextFlyout?.Hide();
    }
}