using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using UI.ViewModels;

namespace UI.Views;

public partial class ProjectDetailsView : UserControl
{
    public ProjectDetailsView()
    {
        InitializeComponent();
    }
    
    public void OnSelectStatusList(object sender, PointerPressedEventArgs e)
    {
        var border = SetStatusBorder;
        var flyout = border.ContextFlyout;
        flyout?.ShowAt(border);
    }

    public void OnStatusSelected(object sender, PointerPressedEventArgs e)
    {
        if (sender is not Border { DataContext: SelectionItemViewModel selectedStatus}) return;

        if (DataContext is ProjectDetailsViewModel viewModel)
        {
            viewModel.SetStatus(selectedStatus.Name);
        }

        var flyout = SetStatusBorder.ContextFlyout;
        flyout?.Hide();
    }

    public void OnSelectPriorityList(object sender, PointerPressedEventArgs e)
    {
        var border = SetPriorityBorder;
        var flyout = border.ContextFlyout;
        flyout?.ShowAt(border);
    }

    public void OnPrioritySelected(object sender, PointerPressedEventArgs e)
    {
        if (sender is not Border { DataContext: SelectionItemViewModel selectedPriority}) return;

        if (DataContext is ProjectDetailsViewModel viewModel)
        {
            viewModel.SetPriority(selectedPriority.Name);
        }
        
        var flyout = SetPriorityBorder.ContextFlyout;
        flyout?.Hide();
    }

    public void OnSelectStartDate(object sender, DatePickerSelectedValueChangedEventArgs e)
    {
        if (DataContext is ProjectDetailsViewModel viewModel && e.NewDate.HasValue)
        {
            viewModel.EditStartDate(e.NewDate.Value.DateTime);
        }
    }

    public void OnSelectDueDate(object sender, DatePickerSelectedValueChangedEventArgs e)
    {
        if (DataContext is ProjectDetailsViewModel viewModel && e.NewDate.HasValue)
        {
            viewModel.EditDueDate(e.NewDate.Value.DateTime);
        }
    }

    public void OnSameDate(object sender, RoutedEventArgs e)
    {
        if (DataContext is ProjectDetailsViewModel viewModel)
        {
            viewModel.CloseDatePicker();
        }
    }

    public void OnDescriptionGotFocus(object sender, GotFocusEventArgs e)
    {
        if (DataContext is ProjectDetailsViewModel viewModel)
        {
            viewModel.ToggleDescriptionButton();
        }
    }

    public void OnDescriptionLostFocus(object sender, RoutedEventArgs e)
    {
        if (DataContext is ProjectDetailsViewModel viewModel)
        {
            viewModel.ToggleDescriptionButton();
        }
    }

    public void OnSaveDescriptionButtonClicked(object sender, RoutedEventArgs e)
    {
        OnDescriptionLostFocus(sender, e);
    }

    public void OnCloseButtonPressed(object? sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<ProjectListView>().FirstOrDefault();
        parent?.OnCloseButtonPressed();
    }
}