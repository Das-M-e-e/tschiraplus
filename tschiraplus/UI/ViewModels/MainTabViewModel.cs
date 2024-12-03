﻿using System.Collections.ObjectModel;
using Services.Repositories;
using Services.TaskServices;
using UI.Views;

namespace UI.ViewModels;

public class MainTabViewModel : ViewModelBase
{
    public ObservableCollection<TabItemModel> Tabs { get; }

    public MainTabViewModel(TaskRepository taskRepository) //Konstruktor
    {
        var taskListViewModel = new TaskListViewModel(new TaskService(taskRepository, new TaskSortingManager()));
        
        Tabs = new ObservableCollection<TabItemModel>
        {
            new("Kanban", new Kanban { DataContext = taskListViewModel }),
            new("ListView", new ListView{ DataContext = taskListViewModel })
        };
    }
}