using FluentValidation.TestHelper;
using TaskHub.Common.DTO.User;
using TaskHub.WebApi.Validators.User;

namespace TaskHub.Tests.TaskHub.WebApi.Validators
{
    [TestFixture]
    public class RegisterModelValidatorTests
    {
        private RegisterModelValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RegisterModelValidator();
        }

        [Test]
        public void Username_WhenTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RegisterModel("user", "test@example.com", "Password123");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Username)
                .WithErrorMessage("Username is too short. Minimum length is 6 symbols.");
        }

        [Test]
        public void Password_WhenNotContainingLetters_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RegisterModel("validusername", "test@example.com", "123456");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password must contain both letters and digits.");
        }

        [Test]
        public void Password_WhenNotContainingUppercase_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RegisterModel("validusername", "test@example.com", "password123");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Passwords must have at least one uppercase ('A'-'Z').");
        }

        [Test]
        public void Email_WhenInvalid_ShouldHaveValidationError()
        {
            // Arrange
            var model = new RegisterModel("validusername", "invalidemail", "Password123");

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email address.");
        }
    }

}
