namespace BlockMaster.Domain.Exceptions.ConflictException;

public class MovieConflictException : ConflictGeneralException
{
    public MovieConflictException(string? message) : base(message)
    {
    }
}