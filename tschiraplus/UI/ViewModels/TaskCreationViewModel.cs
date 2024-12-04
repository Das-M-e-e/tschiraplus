using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Services.DTOs;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    
    
    private TaskDto CreateTask()
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
    
}