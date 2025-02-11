using PetaPoco;
using Services;
using Services.DatabaseServices;
using Services.DTOs;
using Services.Repositories;
using Services.TaskServices;
using Services.UserServices;

namespace TestProject1.TestTaskService;

[TestClass]
public sealed class TestTaskService
{
    private Guid testId = Guid.NewGuid();
    
    private readonly DatabaseService _dbService = new DatabaseService("TestDatabase");
    private readonly ApplicationState _appState = new ApplicationState();
    //private readonly IAuthService _authService;
    
    private ITaskService _taskService;
    private TaskDto _taskDto;
    private ITaskRepository _taskRepository;
    private UserInputParser _userInputParser;
    private DateTime _testDate;
    
    
    public TestTaskService()
    {
        _taskRepository = new TaskRepository(_dbService.GetDatabase(), testId);
        _userInputParser = new UserInputParser();
        _taskService = new TaskService(_taskRepository, new TaskSortingManager(_taskRepository, _appState), _appState, _userInputParser);
        _testDate = DateTime.Now;
        _taskDto = _taskService.CreateTaskDto("Test1", "Test2", "Test3", _testDate);
    }
    
    [TestMethod]
    public void createTaskDto_is_creating_DTO()
    {
       Console.WriteLine("Im working!!! probably"); 
       bool result = _taskDto is not null;
       Assert.IsTrue(result, "An TaskDTO was created");
       Assert.IsTrue(_taskDto.Title == "Test1");
       Assert.IsTrue(_taskDto.Description == "Test2");
       Assert.IsTrue(_taskDto.Status == "Test3");
       Console.WriteLine(_testDate +","+ _taskDto.CreationDate);
       Assert.IsTrue(_taskDto.CreationDate == _testDate);
    }
}