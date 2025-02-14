using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Services.UserServices;

namespace UI.ViewModels;

public class UserProfileViewModel
{
    // Services
    private readonly TokenStorageService _tokenStorageService;
    private readonly WrapperViewModel _wrapper;
    
    // Commands
    public ICommand LogoutCommand { get; set; }
    
    public UserProfileViewModel(WrapperViewModel wrappper)
    {
        _tokenStorageService = new TokenStorageService();
        _wrapper = wrappper;
        LogoutCommand = new RelayCommand(Logout);
    }

    /// <summary>
    /// Logs out the user
    /// </summary>
    private void Logout()
    {
        _tokenStorageService.RemoveToken();
        _wrapper.NavigateToLogin();
    }
}