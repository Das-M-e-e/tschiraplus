using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
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
        if (sender is not Border { DataContext: SelectionItemViewModel selectedStatus }) return;

        if (DataContext is TaskDetailViewModel viewModel)
        {
            viewModel.SetStatus(selectedStatus.Name);

        }

        var flyout = SetStatusBorder.ContextFlyout;
        flyout?.Hide();
    }

    public void OnSelectedPriorityList(object sender, PointerPressedEventArgs args)
    {
        var border = SetPriorityBorder;
        var flyout = border.ContextFlyout;
        flyout?.ShowAt(border);
    }

    public void OnPrioritySelected(object sender, PointerPressedEventArgs args)
    {
        if (sender is not Border { DataContext: SelectionItemViewModel selectedPriority }) return;

        if (DataContext is TaskDetailViewModel viewModel)
        {
            viewModel.SetPriority(selectedPriority.Name);
        }

        var flyout = SetPriorityBorder.ContextFlyout;
        flyout?.Hide();
    }

    public void OnCloseButtonClicked(object sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<TaskListView>().FirstOrDefault();
        parent?.OnCloseButtonPressed();
    }
}