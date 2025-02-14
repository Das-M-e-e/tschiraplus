using Moq;
using Services;
using Services.DTOs;
using Services.TaskServices;
using UI.ViewModels;

namespace Testing.TestUi;
[TestClass]
public class LoadTaskCorrektly
{
    [TestMethod]
    public void LoadTasks_ShouldLoadTasksFromService()
    {
        // Arrange
        var mockTaskService = new Mock<ITaskService>();
        var mockAppState = new Mock<ApplicationState>();
        var projektId = Guid.NewGuid();
        var mainTabViewModel = new MainTabViewModel(mockTaskService.Object, projektId, mockAppState.Object);
       
        var tasks = new List<TaskDto>
        {
            new TaskDto { TaskId = Guid.NewGuid(), Title = "Test Task 1", Status = "Backlog" },
            new TaskDto { TaskId = Guid.NewGuid(), Title = "Test Task 2", Status = "InProgress" }
        };

        mockTaskService.Setup(s => s.GetAllTasks()).Returns(tasks);
        var viewModel = new TaskListViewModel(mockTaskService.Object, mainTabViewModel, mockAppState.Object);

        // Act
        viewModel.LoadTasks();

        // Assert
        Assert.AreEqual(2, viewModel.Tasks.Count);
        Assert.AreEqual("Test Task 1", viewModel.Tasks[0].Title);
        Assert.AreEqual("Test Task 2", viewModel.Tasks[1].Title);
        mockTaskService.Verify(s => s.GetAllTasks(), Times.Once);
    }
}

