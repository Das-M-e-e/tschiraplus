using System.Collections.ObjectModel;
namespace UI.ViewModels;


public class TaskListViewModel
{
    public ObservableCollection<TaskViewModel> Tasks { get; } = new();

    public TaskListViewModel()
    {
        Tasks.Add(new TaskViewModel{Title = "Task 1", Description = "Task 1 description", Status = "Backlog"});
        Tasks.Add(new TaskViewModel{Title = "Task 2", Description = "Task 2 description", Status = "Backlog"});
        Tasks.Add(new TaskViewModel{Title = "Task 3", Description = "Task 3 description", Status = "Backlog"});
        Tasks.Add(new TaskViewModel{Title = "Task 4", Description = "Task 4 description", Status = "Backlog"});
        Tasks.Add(new TaskViewModel{Title = "Task 5", Description = "Task 5 description", Status = "Backlog"});
    }
}