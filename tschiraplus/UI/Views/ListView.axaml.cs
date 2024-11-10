using Avalonia.Controls;
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