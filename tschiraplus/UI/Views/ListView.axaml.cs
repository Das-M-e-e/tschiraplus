using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UI.ViewModels;

namespace UI.Views;

public partial class ListView : UserControl
{
    public ListView()
    {
        InitializeComponent();
        DataContext = new TaskListViewModel();
    }
}