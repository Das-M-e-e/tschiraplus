using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class KanbanView : ReactiveUserControl<TaskListViewModel>
{
    public KanbanView()
    {
        InitializeComponent();
    }
    
}