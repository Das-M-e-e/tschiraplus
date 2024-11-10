using System.Collections.ObjectModel;
using Services.Repositories;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase
{
    public ObservableCollection<TabItemModel> Tabs { get; }

    public MainTabViewModel(TaskRepository taskRepository)
    {
        var taskListViewModel = new TaskListViewModel(taskRepository);
        
        Tabs = new ObservableCollection<TabItemModel>
        {
            new("Kanban", new Kanban { DataContext = taskListViewModel }),
            new("ListView", new ListView{ DataContext = taskListViewModel })
        };
    }
}