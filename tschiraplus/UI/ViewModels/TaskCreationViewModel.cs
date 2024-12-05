using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Services.DTOs;
using Services.Repositories;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    private readonly TaskRepository _taskRepository;
    private readonly TaskService _taskService;

    public TaskCreationViewModel(TaskRepository taskRepository, TaskService taskService) //Konstrukto (evntl. unötig?)
    {
        _taskRepository = taskRepository;
        _taskService = taskService;
    }
    
    //Erstellt eine TaskDTo
    //TODO Bindings zu den entsprechenden Attributen.
    private TaskDto CreateTaskDTO()
    {
        TaskDto dto = new TaskDto()
        {
            TaskId = Guid.NewGuid(),
            Title = string.Empty,
            Description = string.Empty,
            Status = string.Empty,
            CreationDate = DateTime.Today
        };
        
        
        return dto;
    }

    //Stellt TaskDTo in die Datenbank
    private void CreateTask(TaskDto dto)
    {
        _taskRepository.AddTask(                            //Ruft db in Repo auf und speichert Task
            _taskService.convertTaskDtoToTaskModel(dto));   //Wandelt dto zu TaskModell
    }
    
}