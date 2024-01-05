using FluentValidation.TestHelper;
using TaskHub.Common.DTO.Task;
using TaskHub.WebApi.Validators.Task;

namespace TaskHub.Tests.TaskHub.WebApi.Validators
{
    [TestFixture]
    public class UpdateTaskValidatorTests
    {
        private UpdateTaskValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateTaskValidator();
        }

        [Test]
        public void Id_WhenNullOrEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var task = new UpdateTaskDTO { Id = null };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Id)
                .WithErrorMessage("Id is required.");
        }

        [Test]
        public void Id_WhenNotGuid_ShouldHaveValidationError()
        {
            // Arrange
            var task = new UpdateTaskDTO { Id = "not-a-guid" };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Id)
                .WithErrorMessage("Id must be guid.");
        }

        [Test]
        public void Title_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var task = new UpdateTaskDTO { Title = "short" };

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
            var task = new UpdateTaskDTO { Title = new string('a', 256) };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Title)
                .WithErrorMessage("Title can be up to 255 characters long.");
        }

        [Test]
        public void Title_WhenNullOrEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var task = new UpdateTaskDTO { Title = null };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Title)
                .WithErrorMessage("Title is required when updating the task.");
        }

        [Test]
        public void DueDate_WhenNull_ShouldHaveValidationError()
        {
            // Arrange
            var task = new UpdateTaskDTO { DueDate = null };

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
            var task = new UpdateTaskDTO { DueDate = "invalid-date" };

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
            var task = new UpdateTaskDTO { Description = new string('a', 1001) };

            // Act
            var result = _validator.TestValidate(task);

            // Assert
            result.ShouldHaveValidationErrorFor(t => t.Description)
                .WithErrorMessage("Description must be less than 500 characters.");
        }
    }
}
