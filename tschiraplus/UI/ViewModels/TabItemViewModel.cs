using Avalonia.Controls;

namespace UI.ViewModels;

public class TabItemViewModel : ViewModelBase
{
    // Bindings
    public string Header { get; }
    public UserControl Content { get; }
    public object Tag { get; set; }
    public bool CanClose { get; set; } = false;

    public TabItemViewModel(string header, UserControl content)
    {
        Header = header;
        Content = content;
    }
}