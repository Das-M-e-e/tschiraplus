using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Services;

namespace UI.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isDarkTheme;

    public SettingsViewModel()
    {
        _isDarkTheme = ThemeService.Instance.IsDarkTheme;
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeService.Instance.ToggleTheme();
        IsDarkTheme = ThemeService.Instance.IsDarkTheme;
    }
}