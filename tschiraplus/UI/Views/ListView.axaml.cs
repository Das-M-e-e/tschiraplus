using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class ListView : ReactiveUserControl<TaskListViewModel>
{
    public ListView()
    {
        InitializeComponent();
    }
}