using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace UI.Views;

public partial class TaskCreationView : UserControl
{
    public TaskCreationView()
    {
        InitializeComponent();
    }

    public void OnCloseButtonPressed(object? sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<MainTabView>().FirstOrDefault();
        parent?.OnCloseButtonPressed();
    }
}