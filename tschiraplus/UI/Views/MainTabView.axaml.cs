using Avalonia.Controls;

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
    }
}