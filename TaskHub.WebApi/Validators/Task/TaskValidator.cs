using FluentValidation;
using TaskHub.Common.DTO.Task;

namespace TaskHub.WebApi.Validators.Task
{
    public class TaskValidator : AbstractValidator<UpdateTaskDTO>
    {
        public TaskValidator()
        {
            RuleFor(task => task.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(0, 100).WithMessage("Name must be less than 100 characters.");

            RuleFor(task => task.Title)
                .Length(0, 50).WithMessage("Title must be less than 50 characters.");

            RuleFor(task => task.DueDate)
                .Must(BeAValidDate).WithMessage("Invalid Due date!");

            RuleFor(task => task.Description)
                .Length(0, 1000).WithMessage("Description must be less than 500 characters.");
        }

        private bool BeAValidDate(string? value)
        {
            if (value == null) return true;
            DateTime date;
            return DateTime.TryParse(value, out date) && date > DateTime.Now;
        }
    }
}

