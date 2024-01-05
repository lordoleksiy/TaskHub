using FluentValidation.TestHelper;
using TaskHub.Common.DTO.User;
using TaskHub.WebApi.Validators.User;

namespace TaskHub.Tests.TaskHub.WebApi.Validators
{
    [TestFixture]
    public class LoginModelValidatorTests
    {
        private LoginModelValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new LoginModelValidator();
        }

        [Test]
        public void Username_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var model = new LoginModel("short", "ValidPassword123");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                .WithErrorMessage("Username is too short. Minimum length is 6 symbols.");
        }

        [Test]
        public void Username_WhenNullOrEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var model = new LoginModel("user", "ValidPassword123");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                .WithErrorMessage("Username is too short. Minimum length is 6 symbols.");
        }

        [Test]
        public void Password_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var model = new LoginModel("ValidUsername", "short");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password is too short. Minimum length is 6 symbols.");
        }

        [Test]
        public void Password_WhenNoLetters_ShouldHaveValidationError()
        {
            // Arrange
            var model = new LoginModel("ValidUsername", "123456");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must contain both letters and digits.");
        }

        [Test]
        public void Password_WhenNoDigits_ShouldHaveValidationError()
        {
            // Arrange
            var model = new LoginModel("ValidUsername", "Password");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must contain both letters and digits.");
        }

        [Test]
        public void Password_WhenNoUppercase_ShouldHaveValidationError()
        {
            // Arrange
            var model = new LoginModel("ValidUsername", "password123");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Passwords must have at least one uppercase ('A'-'Z').");
        }

        [Test]
        public void Password_WhenNoNonAlphanumeric_ShouldHaveValidationError()
        {
            // Arrange
            var model = new LoginModel("ValidUsername", "Password123");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Passwords must have at least one non-alphanumeric character.");
        }
    }
}
