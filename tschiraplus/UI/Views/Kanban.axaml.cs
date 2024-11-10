using Avalonia.Controls;
using UI.ViewModels;

namespace UI.Views;

public partial class Kanban : UserControl
{
    
    public Kanban()
    {
        InitializeComponent();
        DataContext = new TaskListViewModel();
    }
    
}