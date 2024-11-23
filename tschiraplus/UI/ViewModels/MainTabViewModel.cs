using System.Collections.ObjectModel;
using Services;
using Services.Repositories;
using Services.TaskProcessing;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase
{
    public ObservableCollection<TabItemModel> Tabs { get; }

    public MainTabViewModel(TaskRepository taskRepository)
    {
        var taskListViewModel = new TaskListViewModel(new TaskService(taskRepository, new TaskSortingManager()));
        
        Tabs = new ObservableCollection<TabItemModel>
        {
            new("Kanban", new Kanban { DataContext = taskListViewModel }),
            new("ListView", new ListView{ DataContext = taskListViewModel })
        };
    }
}