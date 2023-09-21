namespace BlockMaster.Domain.Exceptions.BadRequestException;

public class MovieRequestCategoryBadRequestException : BadRequestGeneralException
{
    public MovieRequestCategoryBadRequestException(string? message) : base(message)
    {
    }
}