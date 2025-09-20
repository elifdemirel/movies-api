using FluentValidation;
using Movies.Application.DTOs;

namespace Movies.Api.Validators
{
    public class DirectorCreateValidator : AbstractValidator<DirectorCreateDto>
    {
        public DirectorCreateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.SecondName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Bio)
                .MaximumLength(1000)
                .WithMessage("Bio cannot exceed 1000 characters");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .LessThan(DateTime.Today)
                .WithMessage("BirthDate must be in the past");
        }
    }
}
