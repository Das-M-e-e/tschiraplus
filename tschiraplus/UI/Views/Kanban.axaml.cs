using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class Kanban : ReactiveUserControl<TaskListViewModel>
{
    public Kanban()
    {
        InitializeComponent();
    }
    
}