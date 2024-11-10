using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Controls;

namespace UI.ViewModels;


public class TaskListViewModel
{
    public ObservableCollection<TaskViewModel> Tasks { get; } = new();

    public TaskListViewModel()
    {
        Tasks.Add(new TaskViewModel { Title = "Task 1", Tag = "Example tag1", User = "Franzi" });
        Tasks.Add(new TaskViewModel { Title = "Task 2", Tag = "Example tag2", User = "Sofia" });
        Tasks.Add(new TaskViewModel { Title = "Task 3", Tag = "Example tag3", User = "Franzi" });
        Tasks.Add(new TaskViewModel { Title = "Task 4", Tag = "Example tag4", User = "Sofia" });
        Tasks.Add(new TaskViewModel { Title = "Task 5", Tag = "Example tag5", User = "Franzi" })
            ;
    }
}