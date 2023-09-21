namespace BlockMaster.Domain.Exceptions.BadRequestException;

public class MovieRequestBadRequestException : BadRequestGeneralException
{
    public MovieRequestBadRequestException(string? message) : base(message)
    {
    }
}