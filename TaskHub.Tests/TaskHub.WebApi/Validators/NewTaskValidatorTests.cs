using FluentValidation.TestHelper;
using TaskHub.Common.DTO.Task;
using TaskHub.WebApi.Validators.Task;

namespace TaskHub.Tests.TaskHub.WebApi.Validators
{
    [TestFixture]
    public class NewTaskValidatorTests
    {
        private NewTaskValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new NewTaskValidator();
        }

        [Test]
        public void Title_WhenEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var task = new NewTaskDTO { Title = "" };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Title)
                .WithErrorMessage("Title is required when creating the task.");
        }

        [Test]
        public void Title_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var task = new NewTaskDTO { Title = "short" };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Title)
                .WithErrorMessage("The minimum length of title is 6 symbols");
        }

        [Test]
        public void Title_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var task = new NewTaskDTO { Title = new string('a', 256) };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Title)
                .WithErrorMessage("Title can be up to 255 characters long.");
        }

        // Test for DueDate rule
        [Test]
        public void DueDate_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var task = new NewTaskDTO { DueDate = null };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.DueDate)
                .WithErrorMessage("DueDate is required");
        }

        [Test]
        public void DueDate_WhenInvalidDate_ShouldHaveValidationError()
        {
            // Arrange
            var task = new NewTaskDTO { DueDate = "invalid-date" }; // Assuming DueDate is a string for simplicity

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.DueDate)
                .WithErrorMessage("Invalid Due date!");
        }

        [Test]
        public void Description_WhenTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var task = new NewTaskDTO { Description = new string('a', 1001) };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Description)
                .WithErrorMessage("Description can be up to 1000 characters long.");
        }

        [Test]
        public void ParentTaskId_WhenInvalidGuid_ShouldHaveValidationError()
        {
            // Arrange
            var task = new NewTaskDTO { ParentTaskId = "invalid-guid" }; // Assuming ParentTaskId is a string for simplicity

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.ParentTaskId)
                .WithErrorMessage("Parent task id must be guid.");
        }
    }
}
