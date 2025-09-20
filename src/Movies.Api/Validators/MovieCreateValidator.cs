using FluentValidation;
using Movies.Application.DTOs;
using MongoDB.Bson;

namespace Movies.Api.Validators
{
    public class MovieCreateValidator : AbstractValidator<MovieCreateDto>
    {
        public MovieCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Title must be between 1 and 200 characters");

            RuleFor(x => x.Genre)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ImdbId)
                .NotEmpty()
                .Matches(@"^tt\d{7,8}$")
                .WithMessage("IMDb ID must be in format tt1234567");

            RuleFor(x => x.DirectorId)
                .NotEmpty()
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("Invalid director ID format");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 10);

            RuleFor(x => x.ReleaseDate)
                .LessThanOrEqualTo(DateTime.Today);
        }
    }
}
