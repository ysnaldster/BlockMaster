using System.Globalization;
using BlockMaster.Domain.Enums;
using BlockMaster.Domain.Exceptions.BadRequestException;
using BlockMaster.Domain.Request;
using BlockMaster.Domain.Util;
using FluentValidation;

namespace BlockMaster.Business.Util;

public class MovieRequestValidator : AbstractValidator<MovieRequest>
{
    public MovieRequestValidator()
    {
        const string pattern = "^[a-zA-Z0-9]+(?:\\s[a-zA-Z0-9]+)*$";
        RuleFor(movie => movie.Name)
            .NotEmpty()
            .Matches(pattern)
            .MaximumLength(30);
        RuleFor(movie => movie.Score)
            .InclusiveBetween(0, 5);
        RuleFor(movie => movie.Category)
            .Must(category => ValidateMovieCategory(category!));
    }

    private static bool ValidateMovieCategory(string categoryInput)
    {
        var categoryParseToPascal =
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(categoryInput?.ToLower() ?? string.Empty);
        var validate = Enum.TryParse<CategoriesCollection.MovieCategory>(categoryParseToPascal, out _);
        if (validate)
        {
            return validate;
        }

        throw new MovieRequestCategoryBadRequestException(ExceptionUtil.MovieRequestCategoryBadRequestMessage);
    }
}