using Services.DTOs;
using Services.TaskServices;

namespace Testing.TestTaskServices;

[TestClass]
    public class TestTaskSortingManager
    {
        
        private TaskSortingManager _sortingManager;
        private List<TaskDto> _tasks;

        [TestInitialize]
        public void Setup()
        {
            _sortingManager = new TaskSortingManager();

            _tasks = new List<TaskDto>
            {
                new TaskDto { TaskId = Guid.NewGuid(), Title = "B Task", StartDate = new DateTime(2024, 1, 10), DueDate = new DateTime(2024, 2, 1), Status = "InProgress" },
                new TaskDto { TaskId = Guid.NewGuid(), Title = "A Task", StartDate = new DateTime(2024, 1, 5), DueDate = null, Status = "Ready" },
                new TaskDto { TaskId = Guid.NewGuid(), Title = "C Task", StartDate = new DateTime(2024, 1, 15), DueDate = new DateTime(2024, 3, 1), Status = "InProgress" },
            };
        }
    
        [TestMethod]
        public void SortBySingleAttribute_SortsByTitleAscending()
        {
            var result = _sortingManager.SortBySingleAttribute(_tasks, t => t.Title).ToList();

            Assert.AreEqual("A Task", result[0].Title);
            Assert.AreEqual("B Task", result[1].Title);
            Assert.AreEqual("C Task", result[2].Title);
        }

        [TestMethod]
        public void SortBySingleAttribute_SortsByCreationDateAscending()
        {
            var result = _sortingManager.SortBySingleAttribute(_tasks, t => t.StartDate ?? DateTime.MaxValue).ToList();

            Assert.AreEqual(new DateTime(2024, 1, 5), result[0].StartDate);
            Assert.AreEqual(new DateTime(2024, 1, 10), result[1].StartDate);
            Assert.AreEqual(new DateTime(2024, 1, 15), result[2].StartDate);
        }

        [TestMethod]
        public void SortBySingleAttribute_SortsByDueDate_WithNullValues()
        {
            var result = _sortingManager.SortBySingleAttribute(_tasks, t => t.DueDate ?? DateTime.MaxValue).ToList();
            
            Assert.AreEqual(new DateTime(2024, 2, 1), result[0].DueDate);
            Assert.AreEqual(new DateTime(2024, 3, 1), result[1].DueDate);
            Assert.IsNull(result[2].DueDate);
        }

        [TestMethod]
        public void SortBySingleAttribute_EmptyList_ReturnsEmptyList()
        {
            var result = _sortingManager.SortBySingleAttribute(new List<TaskDto>(), t => t.Title);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void SortBySingleAttribute_SingleElementList_ReturnsSameList()
        {
            var singleTask = new List<TaskDto> { _tasks[0] };
            var result = _sortingManager.SortBySingleAttribute(singleTask, t => t.Title).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(singleTask[0].Title, result[0].Title);
        }
        [TestMethod]
        public void FilterByPredicate_FiltersByStatus()
        {
            var result = _sortingManager.FilterByPredicate(_tasks, t => t.Status == "InProgress").ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(t => t.Status == "InProgress"));
        }

        [TestMethod]
        public void FilterByPredicate_FiltersByCreationDate()
        {
            var result = _sortingManager.FilterByPredicate(_tasks, t => t.StartDate > new DateTime(2024, 1, 7)).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(t => t.StartDate > new DateTime(2024, 1, 7)));
        }

        [TestMethod]
        public void FilterByPredicate_EmptyList_ReturnsEmptyList()
        {
            var result = _sortingManager.FilterByPredicate(new List<TaskDto>(), t => t.Status == "InProgress");

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void FilterByPredicate_NoMatchingElements_ReturnsEmptyList()
        {
            var result = _sortingManager.FilterByPredicate(_tasks, t => t.Status == "NonexistentStatus");

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void FilterByPredicate_AllMatchingElements_ReturnsSameList()
        {
            var result = _sortingManager.FilterByPredicate(_tasks, t => t.StartDate?.Year == 2024).ToList();

            Assert.AreEqual(_tasks.Count, result.Count);
        }
    }