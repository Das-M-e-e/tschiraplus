using Avalonia.Controls;

namespace UI.ViewModels;

public class TabItemModel : ViewModelBase
{
    public string Header { get; }
    public UserControl Content { get; }

    public TabItemModel(string header, UserControl content) //Konstruktor
    {
        Header = header;
        Content = content;
    }
}