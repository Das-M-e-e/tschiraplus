using Avalonia.Controls;
using UI.ViewModels;

namespace UI.Views;

public partial class MainTabView : UserControl
{
    public MainTabView()
    {
        InitializeComponent();
    }

    public void OnCloseButtonPressed()
    {
        Root.ContextFlyout?.Hide();
        var viewModel = DataContext as MainTabViewModel;
        viewModel?.UpdateTaskList();
    }
}