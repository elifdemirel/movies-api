using FluentValidation;
using Movies.Application.DTOs;
using MongoDB.Bson;

namespace Movies.Api.Validators
{
    public class MovieUpdateValidator : AbstractValidator<MovieUpdateDto>
    {
        public MovieUpdateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Title must be between 1 and 200 characters");

            RuleFor(x => x.Genre)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10);

            RuleFor(x => x.ReleaseDate)
                .LessThanOrEqualTo(DateTime.Today);

            RuleFor(x => x.ImdbId)
                .Matches(@"^tt\d{7,8}$")
                .WithMessage("IMDb ID must be in format tt1234567");

            RuleFor(x => x.DirectorId)
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("Invalid director ID format");
        }
    }
}
