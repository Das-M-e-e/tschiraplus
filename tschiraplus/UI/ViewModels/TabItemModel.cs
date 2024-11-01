using Avalonia.Controls;

namespace UI.ViewModels;

public class TabItemModel
{
    public string Header { get; }
    public Window Content { get; }

    public TabItemModel(string header, Window content)
    {
        Header = header;
        Content = content;
    }
}