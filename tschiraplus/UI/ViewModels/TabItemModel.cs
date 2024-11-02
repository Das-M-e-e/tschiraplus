using Avalonia.Controls;

namespace UI.ViewModels;

public class TabItemModel : ViewModelBase
{
    public string Header { get; }
    public UserControl Content { get; }

    public TabItemModel(string header, UserControl content)
    {
        Header = header;
        Content = content;
    }
}