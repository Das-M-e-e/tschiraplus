using Core.Enums;

namespace Core.Models;

public class UserProjectRoleModel
{
    public Guid UserProjectRoleId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public Guid AssignedBy { get; set; }
    public ProjectRole Role { get; set; }
    public DateTime AssignedDate { get; set; }
    public bool IsActive { get; set; }
}