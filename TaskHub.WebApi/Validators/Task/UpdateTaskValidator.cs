using FluentValidation;
using TaskHub.Common.DTO.Task;

namespace TaskHub.WebApi.Validators.Task
{
    public class UpdateTaskValidator : BaseValidator<UpdateTaskDTO>
    {
        public UpdateTaskValidator()
        {
            RuleFor(task => task.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(BeGuid).WithMessage("Id must be guid.");

            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("Title is required when updating the task.")
                .MinimumLength(6).WithMessage("The minimum length of title is 6 symbols")
                .MaximumLength(255).WithMessage("Title can be up to 255 characters long.");

            RuleFor(task => task.DueDate)
                .NotNull().WithMessage("DueDate is required")
                .Must(BeValidDate).WithMessage("Invalid Due date!");

            RuleFor(task => task.Description)
                .Length(0, 1000).WithMessage("Description must be less than 500 characters.");
        }
    }
}

