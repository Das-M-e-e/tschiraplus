using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using UI.ViewModels;

namespace UI.Views;

public partial class CreateNewProjectView : UserControl
{
    public CreateNewProjectView()
    {
        InitializeComponent();
    }

    public void OnCreateProjectButtonPressed(object? sender, RoutedEventArgs args)
    {
        if (DataContext is not CreateNewProjectViewModel viewModel) return;
        if (viewModel.CreateProject())
        {
            OnCloseButtonPressed(sender, args);
        }
    }

    public void OnCloseButtonPressed(object? sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<ProjectListView>().FirstOrDefault();
        parent?.OnCloseButtonPressed();
    }
}