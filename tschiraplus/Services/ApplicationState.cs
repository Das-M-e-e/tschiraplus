using Services.DTOs;

namespace Services;

public class ApplicationState
{
    public UserDTO CurrentUser { get; set; }
    public Guid? CurrentProjectId { get; set; }
}