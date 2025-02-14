using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using UI.ViewModels;

namespace UI.Views;

public partial class TaskCreationView : UserControl
{
    public TaskCreationView()
    {
        InitializeComponent();
    }

    public void OnSelectPriorityList(object sender, PointerPressedEventArgs args)
    {
        var border = SelectPriorityBorder;
        var flyout = border.ContextFlyout;
        flyout?.ShowAt(border);
    }

    public void OnPrioritySelected(object sender, PointerPressedEventArgs args)
    {
        if (sender is not Border { DataContext: SelectionItemViewModel selectedPriority }) return;

        if (DataContext is TaskCreationViewModel viewModel)
        {
            viewModel.Priority = selectedPriority;
        }

        var flyout = SelectPriorityBorder.ContextFlyout;
        flyout?.Hide();
    }
    
    public void OnSelectTagsList(object sender, PointerPressedEventArgs args)
    {
        var border = SelectTagBorder;
        var flyout = border.ContextFlyout;
        flyout?.ShowAt(border);
    }

    public void OnTagsSelected(object sender, PointerPressedEventArgs args)
    {
        if (sender is not Border { DataContext: SelectionItemViewModel selectedTag }) return;

        if (DataContext is TaskCreationViewModel viewModel)
        {
            viewModel.CurrentTag = selectedTag;
        }

        var flyout = SelectTagBorder.ContextFlyout;
        flyout?.Hide();
    }

    public void OnCreateTaskButtonPressed(object? sender, RoutedEventArgs args)
    {
        if (DataContext is not TaskCreationViewModel viewModel) return;
        if (viewModel.CreateTask())
        {
            OnCloseButtonPressed(sender, args);
        }
    }

    public void OnCloseButtonPressed(object? sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<MainTabView>().FirstOrDefault();
        parent?.OnCloseButtonPressed();
    }
}