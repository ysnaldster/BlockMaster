using BlockMaster.Domain.Request;
using FluentValidation;

namespace BlockMaster.Business.Util;

public class MovieRequestValidator : AbstractValidator<MovieRequest>
{
    public MovieRequestValidator()
    {
        RuleFor(movie => movie.Name)
            .NotEmpty().WithMessage("El nombre de la película no puede estar vacío.")
            .Matches($"^[a-zA-Z0-9]+(?:\\s[a-zA-Z0-9]+)*$")
            .MaximumLength(30).WithMessage("El nombre de la película debe tener como máximo 30 caracteres.")
            .WithMessage("El nombre de la película solo puede contener letras, números y espacios.");
        RuleFor(movie => movie.Score)
            .InclusiveBetween(0, 5).WithMessage("El valor no puede exceder de 5.");
    }
}