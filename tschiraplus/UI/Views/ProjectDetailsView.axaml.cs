using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;

namespace UI.Views;

public partial class ProjectDetailsView : UserControl
{
    public ProjectDetailsView()
    {
        InitializeComponent();
    }

    public void OnCloseButtonPressed(object? sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<ProjectListView>().FirstOrDefault();
        parent?.OnCloseButtonPressed();
    }

    public void OnSelectStatusList(object sender, PointerPressedEventArgs args)
    {
        
    }
}