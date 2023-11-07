using FluentValidation;
using TaskHub.Common.DTO.User;

namespace TaskHub.WebApi.Validators.User
{
    public class RegisterModelValidator: AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator() 
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Username is too short. Minimum length is 6 symbols.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password is too short. Minimum length is 6 symbols.")
                .Matches("^(?=.*[A-Za-z])(?=.*\\d).+").WithMessage("Password must contain both letters and digits.")
                .Must(password => password.Any(char.IsUpper)).WithMessage("Passwords must have at least one uppercase ('A'-'Z').")
                .Must(password => password.Any(ch => !char.IsLetterOrDigit(ch))).WithMessage("Passwords must have at least one non-alphanumeric character.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email address.");
        }
    }
}
