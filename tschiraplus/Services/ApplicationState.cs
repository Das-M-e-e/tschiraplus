using Services.DTOs;

namespace Services;

public class ApplicationState
{
    public UserDto? CurrentUser { get; set; }
    public Guid? CurrentProjectId { get; set; }
    public bool IsOnline { get; set; }
}