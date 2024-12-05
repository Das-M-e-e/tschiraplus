using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class TaskListView : ReactiveUserControl<TaskListViewModel>
{
    public TaskListView()
    {
        InitializeComponent();
    }
}