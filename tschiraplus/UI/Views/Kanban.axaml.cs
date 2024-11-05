using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.InteropServices;
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