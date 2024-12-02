using Core.Models;
using PetaPoco;
using Services.DTOs;

namespace Services.Repositories;

public class ProjectRepository
{
    private readonly Database _db;

    public ProjectRepository(Database db)
    {
        _db = db;
    }

    public void AddProject(ProjectModel project)
    {
        _db.Insert("Projects", "ProjectId", project);
    }

    public ProjectModel GetProjectById(Guid projectId)
    {
        return _db.SingleOrDefault<ProjectModel>("SELECT * FROM Projects WHERE ProjectId=@0", projectId);
    }

    public List<ProjectDTO> GetAllProjects()
    {
        var projects = _db.Fetch<ProjectModel>("SELECT * FROM Projects");

        return projects.Select(project => new ProjectDTO
        {
            ProjectId = project.ProjectId,
            Name = project.Name,
            Description = project.Description ?? "No description provided..."
        }).ToList();
    }

    public void DeleteProject(Guid projectId)
    {
        _db.Execute("DELETE FROM Projects WHERE ProjectId=@0", projectId);
    }
}