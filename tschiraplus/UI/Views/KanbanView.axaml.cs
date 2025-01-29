using System.Linq;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class KanbanView : ReactiveUserControl<TaskListViewModel>
{
    public KanbanView()
    {
        InitializeComponent();
    }

    public void OnCreateTaskButtonPressed(object? sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<MainTabView>().FirstOrDefault();
        var tabView = parent?.Root;
        var flyout = tabView?.ContextFlyout;
        flyout?.ShowAt(tabView!);
    }
}