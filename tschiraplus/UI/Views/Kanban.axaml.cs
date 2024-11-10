using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using UI.ViewModels;

namespace UI.Views;

public partial class Kanban : UserControl
{
    private Point _ghostPosition = new(0, 0);
    private readonly Point _mouseOffset = new(-5, -5);
    
    public Kanban()
    {
        InitializeComponent();
        DataContext = new TaskListViewModel();
        
    }
    
}