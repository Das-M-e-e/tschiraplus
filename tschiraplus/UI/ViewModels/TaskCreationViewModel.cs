using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.DTOs;
using Services.TaskServices;

namespace UI.ViewModels;

public class TaskCreationViewModel : ViewModelBase
{
    // Services
    private readonly ITaskService _taskService;
    private readonly MainTabViewModel _mainTabViewModel;

    // Bindings
    public bool IsLowPrio {get; set;}
    public bool IsHighPrio {get; set;}
    public bool IsMediumPrio {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    
    public string InitialStatus {get; set;}
   /* public Guid TagId {get; set;}
    public string TagTitle {get; set;}
    public string TagDescription {get; set;}
    public string ColorCode {get; set;} */
    
    // Commands
    public ICommand CreateTaskCommand { get; }
    
    public ICommand ApplyTagCommand { get; }

    public TaskCreationViewModel(ITaskService taskService, MainTabViewModel mainTabViewModel)
    {
        _taskService = taskService;
        _mainTabViewModel = mainTabViewModel;
        CreateTaskCommand = new RelayCommand(CreateTask);
        ApplyTagCommand = new RelayCommand(ApplyTag);

    }

    /// <summary>
    /// Uses the _taskService to create a new task
    /// </summary>
    private void CreateTask()
    {
        var dto = _taskService.CreateTaskDto(
            Title,
            Description,
            InitialStatus,
            "NotSet",
            DateTime.Today);
        _taskService.CreateTask(dto);
        _mainTabViewModel.SelectedTabIndex = 0;
        _mainTabViewModel.CloseCurrentTab();
    }

    private void ApplyTag()
    {
        /*
        var dto = _taskService.ApplyTagDto(
            TagId,
            TagTitle,
            TagDescription,
            ColorCode );
        _taskService.ApplyTag(dto); */

        var Documentation_Tag = new TagDto { TagId = Guid.NewGuid(), Title = "Documentation", ColorCode = "Blue" };
        var Enhancement_Tag = new TagDto
            { TagId = Guid.NewGuid(), Title = "Enhancement", Description = null, ColorCode = "violet" };
        var Help_wanted_Tag = new TagDto
            { TagId = Guid.NewGuid(), Title = "Help Wanted", Description = null, ColorCode = "orange" };
        var Invalid_Tag = new TagDto
            { TagId = Guid.NewGuid(), Title = "Invalid", Description = null, ColorCode = "yellow" };
        var Needs_review_Tag = new TagDto
            { TagId = Guid.NewGuid(), Title = "Needs Review", Description = null, ColorCode = "green" };
        var Not_sprint_relevant_Tag = new TagDto
            { TagId = Guid.NewGuid(), Title = "Not Sprint Relevant", Description = null, ColorCode = "turquoise" };
        var Question_Tag = new TagDto
            { TagId = Guid.NewGuid(), Title = "Question", Description = null, ColorCode = "pink" };
        var Other_Tag = new TagDto { TagId = Guid.NewGuid(), Title = "Other", Description = null, ColorCode = "grey" };

    }

/*
   public ObservableCollection<TaskCreationViewModel> SelectedTags { get; } = [];
   public ObservableCollection<TaskCreationViewModel> AvailableTags { get; } = [
       Documentation_Tag, Enhancement_Tag, Help_wanted_Tag, Invalid_Tag, Needs_review_Tag, Not_sprint_relevant_Tag, Question_Tag, Other_Tag];
   */
}

