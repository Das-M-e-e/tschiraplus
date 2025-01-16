using Avalonia.Controls;
using Avalonia.Input;
using UI.ViewModels;

namespace UI.Views;

public partial class TaskDetailView : UserControl
{
    public TaskDetailView()
    {
        InitializeComponent();
    }

    public void OnSelectStatusList(object sender, PointerPressedEventArgs args)
    {
        var border = SetStatusBorder;
        var flyout = border.ContextFlyout;
        flyout?.ShowAt(border);
    }

    public void OnStatusSelected(object sender, PointerPressedEventArgs args)
    {
        if (sender is not Border { DataContext: SelectionItemViewModel selectedStatus}) return;

        if (DataContext is TaskDetailViewModel viewModel)
        {
            viewModel.SetStatus(selectedStatus.Name);
            
        }
        
        var flyout = SetStatusBorder.ContextFlyout;
        flyout?.Hide();
    }
}