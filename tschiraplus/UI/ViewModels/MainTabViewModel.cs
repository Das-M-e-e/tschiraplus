using System.Collections.ObjectModel;
using Services;
using Services.DatabaseServices;
using Services.Repositories;
using Services.TaskServices;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase, IParameterizedViewModel
{
    private TaskRepository _taskRepository;
    
    public ObservableCollection<TabItemViewModel> Tabs { get; }

    public MainTabViewModel(TaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
        var taskListViewModel = new TaskListViewModel(new TaskService(_taskRepository, new TaskSortingManager()));
        
        Tabs = new ObservableCollection<TabItemViewModel>
        {
            new("KanbanView", new KanbanView { DataContext = taskListViewModel }),
            new("TaskListView", new TaskListView{ DataContext = taskListViewModel })
        };
    }

    public void Initialize(object parameter)
    {
        if (parameter is DatabaseService databaseService)
        {
            _taskRepository = new TaskRepository(databaseService.GetDatabase());
        }
    }
}