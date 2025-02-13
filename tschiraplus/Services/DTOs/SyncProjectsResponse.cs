using Core.Models;

namespace Services.DTOs;

public class SyncProjectsResponse
{
    public UserModel User { get; set; }
    public List<ProjectUserModel> ProjectUsers { get; set; }
    public List<ProjectModel> Projects { get; set; }
}