using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase
{
    public ObservableCollection<TabItemModel> Tabs { get; }

    public MainTabViewModel()
    {
        Tabs = new ObservableCollection<TabItemModel>
        {
            new TabItemModel("Kanban", new Kanban()),
            new TabItemModel("ListView", new ListView())
        };
    }
}