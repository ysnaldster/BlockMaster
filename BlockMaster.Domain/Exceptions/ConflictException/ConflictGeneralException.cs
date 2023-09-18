namespace BlockMaster.Domain.Exceptions.ConflictException;

public abstract class ConflictGeneralException : GeneralException
{
    protected ConflictGeneralException(string? message) : base(message)
    {
    }
}