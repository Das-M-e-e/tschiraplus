using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace UI.ViewModels;

public class MenuProjectViewModel
{
    // Services
    private readonly MainMenuViewModel _mainMenuViewModel;
    
    // Bindings
    private Guid ProjectId { get; }
    public string Name { get; set; }
    
    // Commands
    public ICommand OpenProjectCommand { get; set; }

    public MenuProjectViewModel(Guid projectId, string name, MainMenuViewModel mainMenuViewModel)
    {
        _mainMenuViewModel = mainMenuViewModel;
        
        ProjectId = projectId;
        Name = name;

        OpenProjectCommand = new RelayCommand(OpenProject);
    }

    private void OpenProject()
    {
        _mainMenuViewModel.OpenProject(ProjectId);
    }
}