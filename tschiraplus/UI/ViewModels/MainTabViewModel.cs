using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase
{
    public ObservableCollection<TabItemModel> Tabs { get; }

    public MainTabViewModel()
    {
        Tabs = new ObservableCollection<TabItemModel>
        {
            new TabItemModel("One", new Window()),
            new TabItemModel("Two", new Window())
        };
    }
}