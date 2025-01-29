using System.Threading.Tasks;
using Avalonia.Controls;
using UI.ViewModels;

namespace UI.Views;

public partial class MainTabView : UserControl
{
    public MainTabView()
    {
        InitializeComponent();
    }

    public async void OnCloseButtonPressed()
    {
        Root.ContextFlyout?.Hide();
        if (DataContext is MainTabViewModel viewModel)
        {
            await Task.Delay(20);
            viewModel.CloseFlyout();
        }
    }
}