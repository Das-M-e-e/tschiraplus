using Core.Models;
using Newtonsoft.Json;
using Services.DTOs;
using Services.Repositories;

namespace Services.DatabaseServices;

public class SyncService
{
    private readonly RemoteDatabaseService _remoteDatabaseService;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectUserRepository _projectUserRepository;
    private readonly ITaskRepository _taskRepository;
    
    private CancellationTokenSource? _projectSyncTokenSource;
    private CancellationTokenSource? _taskSyncTokenSource;

    public SyncService(RemoteDatabaseService remoteDatabaseService, IUserRepository userRepository,
        IProjectRepository projectRepository, IProjectUserRepository projectUserRepository, ITaskRepository taskRepository)
    {
        _remoteDatabaseService = remoteDatabaseService;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _projectUserRepository = projectUserRepository;
        _taskRepository = taskRepository;
    }

    public async Task SyncProjectsAsync(Guid userId)
    {
        try
        {
            var response = await _remoteDatabaseService.GetByIdAsync("sync/projects", userId);
            if (string.IsNullOrEmpty(response))
            {
                Console.WriteLine("No data received from the remote database.");
                return;
            }

            var syncData = JsonConvert.DeserializeObject<SyncProjectsResponse>(response);

            if (syncData?.User != null)
            {
                var localUser = _userRepository.GetUserById(syncData.User.UserId);
                if (localUser == null)
                {
                    _userRepository.AddUser(syncData.User);
                }
            }
            
            if (syncData?.Projects != null)
            {
                foreach (var project in syncData.Projects)
                {
                    var localProject = _projectRepository.GetProjectById(project.ProjectId);
                    if (localProject == null)
                    {
                        _projectRepository.AddProject(project);
                    }
                    else if (project.LastUpdated > localProject.LastUpdated)
                    {
                        _projectRepository.UpdateProject(project);
                    }
                }
            }

            if (syncData?.ProjectUsers != null)
            {
                foreach (var projectUser in syncData.ProjectUsers)
                {
                    var localProjectUser = _projectUserRepository.GetProjectUserById(projectUser.ProjectUserId);
                    if (localProjectUser == null)
                    {
                        _projectUserRepository.AddProjectUser(projectUser);
                    }
                }
            }
            Console.WriteLine("Syncing projects...");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during sync: {e.Message}");
        }
    }

    public async Task SyncTasksAsync(Guid projectId)
    {
        try
        {
            var response = await _remoteDatabaseService.GetByIdAsync("sync/tasks", projectId);
            if (string.IsNullOrEmpty(response))
            {
                Console.WriteLine("No data received from the remote database.");
                return;
            }

            var syncData = JsonConvert.DeserializeObject<List<TaskModel>>(response);

            if (syncData == null || syncData.Count == 0)
            {
                Console.WriteLine("No tasks found for project.");
                return;
            }

            foreach (var task in syncData)
            {
                var localTask = _taskRepository.GetTaskById(task.TaskId);
                if (localTask == null)
                {
                    _taskRepository.AddTask(task);
                }
                else if (task.LastUpdated > localTask.LastUpdated)
                {
                    _taskRepository.UpdateTask(task);
                }
            }
            Console.WriteLine("Syncing tasks...");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during sync: {e.Message}");
        }
    }

    public void StartProjectSync(Guid userId)
    {
        StopProjectSync();
        
        _projectSyncTokenSource = new CancellationTokenSource();
        var token = _projectSyncTokenSource.Token;
        
        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                await SyncProjectsAsync(userId);
                await Task.Delay(TimeSpan.FromSeconds(30), token);
            }
        }, token);
    }

    public void StopProjectSync()
    {
        _projectSyncTokenSource?.Cancel();
        _projectSyncTokenSource?.Dispose();
        _projectSyncTokenSource = null;
    }

    public void StartTaskSync(Guid projectId)
    {
        StopTaskSync();
        
        _taskSyncTokenSource = new CancellationTokenSource();
        var token = _taskSyncTokenSource.Token;
        
        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                await SyncTasksAsync(projectId);
                await Task.Delay(TimeSpan.FromSeconds(30), token);
            }
        }, token);
    }

    public void StopTaskSync()
    {
        _taskSyncTokenSource?.Cancel();
        _taskSyncTokenSource?.Dispose();
        _taskSyncTokenSource = null;
    }
}