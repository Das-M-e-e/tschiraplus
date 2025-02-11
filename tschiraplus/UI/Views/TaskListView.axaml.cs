using System.Linq;
using Avalonia.Interactivity;
using Avalonia.Input;
using Avalonia.LogicalTree;
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
        var parent = this.GetLogicalAncestors().OfType<MainTabView>().FirstOrDefault();
        var tabView = parent?.Root;
        var flyout = tabView?.ContextFlyout;
        flyout?.ShowAt(tabView!);
    }

    private void SearchBar_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        if (DataContext is not TaskListViewModel vm) return;
        if (vm.ManipulateTasksCommand.CanExecute(null))
        {
            vm.ManipulateTasksCommand.Execute(null);
        }
    }
} 