using Services;
using Services.DatabaseServices;
using Services.TaskServices;

namespace Testing.TestTaskServices;

[TestClass]
public sealed class TestTaskService
{
    private Guid _testId;
    private DateTime _testDate;

    
    private readonly DatabaseService _dbService = new DatabaseService("TestDatabase");
    private readonly ApplicationState _appState = new ApplicationState();
    //private readonly IAuthService _authService;
    
    private ITaskService _taskService;
    
    
    public TestTaskService()
    {
        
        _testDate = DateTime.Now;
        _testId = Guid.NewGuid();
        
    }
    
    [TestMethod]
    public void TestcreateTask()
    {
      
    }
}