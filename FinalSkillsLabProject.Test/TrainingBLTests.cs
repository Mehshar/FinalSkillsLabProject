using FinalSkillsLabProject.BL.BusinessLogicLayer;
using FinalSkillsLabProject.BL.Interfaces;
using FinalSkillsLabProject.Common.Models;
using FinalSkillsLabProject.DAL.Interfaces;
using FluentAssertions;
using Moq;

namespace FinalSkillsLabProject.Test
{
    [TestFixture]
    public class TrainingBLTests
    {
        private Mock<ITrainingDAL> _stubTraining;
        private ITrainingBL _trainingBL;

        private List<TrainingModel> _trainings;
        private List<int> _prerequisitesList;

        [SetUp]
        public void Setup()
        {
            _trainings = new List<TrainingModel>()
            {
                new TrainingModel(){TrainingId = 1,TrainingName = "Advanced Software Development Bootcamp",Description = "An intensive bootcamp that delves into advanced software development concepts, tools, and methodologies, preparing participants for complex coding challenges.",Deadline = new DateTime(2023, 12, 15),Capacity = 3,PriorityDepartment = 1,PriorityDepartmentName = "Product & Technology",IsDeleted = false},
                new TrainingModel(){TrainingId = 2,TrainingName = "Advanced Technical Support Training",Description = "A training tailored for support engineers focusing on advanced troubleshooting, customer communication skills, and in-depth product knowledge.",Deadline = new DateTime(2024, 11, 11),Capacity = 3,PriorityDepartment = 6,PriorityDepartmentName = "Support & Services",IsDeleted = true},
                new TrainingModel(){TrainingId = 3,TrainingName = "Strategic HR Leadership Program",Description = "A leadership program for HR professionals, covering strategic HR planning, employee engagement strategies, and leadership development.",Deadline = new DateTime(2024, 10, 08),Capacity = 5,PriorityDepartment = 2,PriorityDepartmentName = "Human Resources",IsDeleted = false}
            };

            _prerequisitesList = new List<int>() { 1, 2 };

            _stubTraining = new Mock<ITrainingDAL>();

            _stubTraining.Setup(trainingDAL => trainingDAL.GetAllAsync())
                .ReturnsAsync(_trainings);

            _stubTraining.Setup(trainingDAL => trainingDAL.AddAsync(It.IsAny<TrainingModel>(), It.IsAny<List<int>>()))
                .Returns(Task.CompletedTask);

            _stubTraining.Setup(trainingDAL => trainingDAL.UpdateAsync(It.IsAny<TrainingPrerequisiteViewModel>()))
                .Returns(Task.CompletedTask);

            _stubTraining.Setup(trainingDAL => trainingDAL.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int trainingId) =>
                {
                    TrainingModel training = _trainings.First(x => x.TrainingId == trainingId);
                    _trainings.Remove(training);
                    return true;
                });

            _stubTraining.Setup(trainingDAL => trainingDAL.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((int trainingId) => _trainings.FirstOrDefault(x => x.TrainingId == trainingId));

            _stubTraining.Setup(trainingDAL => trainingDAL.GetByDeadlineAsync())
                .ReturnsAsync(() => _trainings.Where(x => x.Deadline <= DateTime.Now).ToList());

            _trainingBL = new TrainingBL(_stubTraining.Object);
        }

        [Test]
        public async Task AddAsync_ValidTraining_ReturnsSuccessMessage()
        {
            // Arrange
            var training = new TrainingModel()
            {
                TrainingName = "Training X",
                Description = "some description for Training X.",
                Deadline = new DateTime(2024, 12, 15),
                Capacity = 3,
                PriorityDepartment = 1
            };

            // Act
            var result = await _trainingBL.AddAsync(training, _prerequisitesList);

            // Assert
            Assert.That(result, Does.Contain("Training created successfully!"));
        }

        [Test]
        public async Task AddAsync_DuplicateName_ReturnsErrorMessage()
        {
            // Arrange
            var training = new TrainingModel()
            {
                TrainingName = "Advanced Software Development Bootcamp",
                Description = "some description for the training.",
                Deadline = new DateTime(2024, 12, 20),
                Capacity = 3,
                PriorityDepartment = 1
            };

            // Act
            var result = await _trainingBL.AddAsync(training, _prerequisitesList);

            // Assert
            Assert.That(result, Does.Contain("Training name already exists!"));
        }

        [Test]
        [TestCase("Training Y", new[] { 1, 2 }, "Training updated successfully!")]
        [TestCase("Advanced Software Development Bootcamp", new[] { 1, 3 }, "Training name already exists!")]
        public async Task UpdateAsync_ValidInvalid_SuccessFailure(string trainingName, int[] prerequisiteIds, string expectedResult)
        {
            // Arrange
            var training = new TrainingPrerequisiteViewModel()
            {
                TrainingId = 20,
                TrainingName = trainingName,
                Description = "some description for the training",
                Deadline = new DateTime(2024, 12, 25),
                Capacity = 3,
                PriorityDepartment = 1,
                PrerequisiteIds = prerequisiteIds.ToList()
            };

            // Act
            var result = await _trainingBL.UpdateAsync(training);

            // Assert
            Assert.That(result, Does.Contain(expectedResult));
        }

        [Test]
        public async Task DeleteAsync_Valid_ReturnsTrue()
        {
            // Arrange
            int trainingId = 1;

            // Act
            var result = await _trainingBL.DeleteAsync(trainingId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetAsync_SuccessfulRetrieval_ReturnsTraining()
        {
            // Arrange
            var trainingId = 2;

            var expectedResult = new TrainingModel()
            {
                TrainingId = 2,
                TrainingName = "Advanced Technical Support Training",
                Description = "A training tailored for support engineers focusing on advanced troubleshooting, customer communication skills, and in-depth product knowledge.",
                Deadline = new DateTime(2024, 11, 11),
                Capacity = 3,
                PriorityDepartment = 6,
                PriorityDepartmentName = "Support & Services",
                IsDeleted = true
            };

            // Act
            var result = await _trainingBL.GetAsync(trainingId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetByDeadlineAsync_SuccessfulRetrieval_ReturnsTraining()
        {
            // Arrange
            var expectedResult = new List<TrainingModel>()
            {
                new TrainingModel()
                {
                    TrainingId = 1,
                    TrainingName = "Advanced Software Development Bootcamp",
                    Description = "An intensive bootcamp that delves into advanced software development concepts, tools, and methodologies, preparing participants for complex coding challenges.",
                    Deadline = new DateTime(2023, 12, 15),
                    Capacity = 3,
                    PriorityDepartment = 1,
                    PriorityDepartmentName = "Product & Technology",
                    IsDeleted = false
                }
            };

            // Act
            var result = (await _trainingBL.GetByDeadlineAsync()).ToList();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
