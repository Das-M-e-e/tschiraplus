using Avalonia.Controls;

namespace UI.ViewModels;

public class TabItemViewModel : ViewModelBase
{
    public string Header { get; }
    public UserControl Content { get; }
    public object Tag { get; set; }

    public TabItemViewModel(string header, UserControl content)
    {
        Header = header;
        Content = content;
    }
}