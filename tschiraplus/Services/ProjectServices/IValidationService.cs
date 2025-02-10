namespace Services.ProjectServices;

public interface IValidationService
{
    bool ValidateUsername(string username, out string errorMessage);
    bool ValidateEmail(string email, out string errorMessage);
    bool ValidatePassword(string password, out string errorMessage);
    bool ValidateConfirmPassword(string password, string confirmPassword, out string errorMessage);
}