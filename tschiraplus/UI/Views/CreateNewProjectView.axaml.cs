using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace UI.Views;

public partial class CreateNewProjectView : UserControl
{
    public CreateNewProjectView()
    {
        InitializeComponent();
    }

    public void OnCloseButtonPressed(object? sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<ProjectListView>().FirstOrDefault();
        parent?.OnCloseButtonPressed();
    }
}