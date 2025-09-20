using FluentValidation;
using Movies.Application.DTOs;

namespace Movies.Api.Validators
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequestDto>
    {
        public PaginationRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must be greater than or equal to 1");

            RuleFor(x => x.Size)
                .InclusiveBetween(1, 100)
                .WithMessage("Size must be between 1 and 100");
        }
    }
}
