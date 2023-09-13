namespace BlockMaster.Domain.Exceptions.NotFoundException;

public abstract class NotFoundGeneralException : GeneralException
{
    protected NotFoundGeneralException(string? message) : base(message)
    {
    }
}