using FluentValidation;
using TaskHub.Common.DTO.Task;

namespace TaskHub.WebApi.Validators.Task
{
    public class NewTaskValidator : AbstractValidator<NewTaskDTO>
    {
        public NewTaskValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("Title is required when creating the task.")
                .MinimumLength(6).WithMessage("The minimum length of title is 6 symbols")
                .MaximumLength(255).WithMessage("Title can be up to 255 characters long.");

            RuleFor(task => task.DueDate)
                .NotNull().WithMessage("DueDate is required")
                .Must(BeAValidDate).WithMessage("Invalid Due date!");

            RuleFor(task => task.Description)
                .MaximumLength(1000).WithMessage("Description can be up to 1000 characters long.");
        }

        private bool BeAValidDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date) && date > DateTime.Now;
        }
    }
}
