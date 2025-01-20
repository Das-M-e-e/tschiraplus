using Avalonia.Input;
using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class TaskListView : ReactiveUserControl<TaskListViewModel>
{
    public TaskListView()
    {
        InitializeComponent();
    }

    private void SearchBar_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        if (DataContext is not TaskListViewModel vm) return;
        if (vm.ManipulateTasksCommand.CanExecute(null))
        {
            vm.ManipulateTasksCommand.Execute(null);
        }
    }
} 