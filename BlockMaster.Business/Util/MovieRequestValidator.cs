using BlockMaster.Domain.Request;
using FluentValidation;

namespace BlockMaster.Business.Util;

public class MovieRequestValidator : AbstractValidator<MovieRequest>
{
    public MovieRequestValidator(int movieName)
    {
        RuleFor(movie => movie.Name)
            .NotEmpty().WithMessage("El nombre de la película no puede estar vacío.")
            .Matches($"^[a-zA-Z0-9]+(?:\\s[a-zA-Z0-9]+)*$")
            .MaximumLength(30).WithMessage("El nombre de la película debe tener como máximo 25 caracteres.")
            .WithMessage("El nombre de la película solo puede contener letras, números y espacios.");
        RuleFor(movie => movie.Score)
            .InclusiveBetween(0, 5).WithMessage("El valor no puede exceder de 5.");
    }

    public MovieRequestValidator(int requestCountryCode, int originalCountryCode)
    {
        RuleFor(movie => movie.CountryCode)
            .Must(request => ValidateCountryCode(requestCountryCode, originalCountryCode));
    }

    private static bool ValidateCountryCode(int? requestCountryCode, int originalCountryCode)
    {
        var requestCountryCodeAsString = requestCountryCode.ToString();
        return requestCountryCodeAsString?.Substring(0, 2) == originalCountryCode.ToString();
    }
}