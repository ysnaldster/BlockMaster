namespace BlockMaster.Domain.Exceptions.NotFoundException;

public class MovieNotFoundException : NotFoundGeneralException
{
    public MovieNotFoundException(string message) : base(message)
    {
    }
}