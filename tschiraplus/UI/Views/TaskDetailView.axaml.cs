using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Styling;
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

    public void OnSelectStartDate(object sender, DatePickerSelectedValueChangedEventArgs args)
    {
        if (DataContext is TaskDetailViewModel viewModel && args.NewDate.HasValue)
        {
            viewModel.EditStartDate(args.NewDate.Value.DateTime);
        }
    }
    
    public void OnSelectDueDate(object sender, DatePickerSelectedValueChangedEventArgs args)
    {
        if (DataContext is TaskDetailViewModel viewModel && args.NewDate.HasValue)
        {
            viewModel.EditDueDate(args.NewDate.Value.DateTime);
        }
    }

    public void OnSameDate(object sender, RoutedEventArgs args)
    {
        if (DataContext is TaskDetailViewModel viewModel)
        {
            viewModel.CloseDatePicker();
        }
    }
    
    public async Task RunDescriptionBorderAnimation()
    {
        OnDescriptionLostFocus();
        var normalColor = DescriptionTextBox.BorderBrush;
        if (DescriptionTextBox.Text != "")
        {
            DescriptionTextBox.BorderBrush = Brushes.PaleGreen;
            
            await Task.Delay(800);
            
            var animation = new Animation
            {
                Duration = TimeSpan.FromSeconds(1),
                Easing = new CubicEaseInOut(),
                Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(0),
                        Setters = { new Setter(BorderBrushProperty, Brushes.PaleGreen) }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(BorderBrushProperty, new SolidColorBrush(Color.Parse("#dfe3e8")))
                        }
                    }
                }
            };
            
            await animation.RunAsync(DescriptionTextBox);
            DescriptionTextBox.BorderBrush = normalColor;
            
        }
    }

    public void OnDescriptionGotFocus(object sender, GotFocusEventArgs args)
    {
        if (DataContext is TaskDetailViewModel viewModel)
        {
            viewModel.ToggleDescriptionButton();
        }
    }
    
    private void OnDescriptionLostFocus()
    {
        if (DataContext is TaskDetailViewModel viewModel)
        {
            viewModel.ToggleDescriptionButton();
        }
    }
    
    public void OnCloseButtonClicked(object sender, RoutedEventArgs args)
    {
        var parent = this.GetLogicalAncestors().OfType<MainTabView>().FirstOrDefault();
        parent?.OnCloseButtonPressed();
        var vm = parent?.DataContext as MainTabViewModel;
        vm!.TaskCheckBoxClicked = false;
    }

    private void OnCommentSaveButtonClick(object? sender, RoutedEventArgs e)
    {
        RunDescriptionBorderAnimation();
    }
}