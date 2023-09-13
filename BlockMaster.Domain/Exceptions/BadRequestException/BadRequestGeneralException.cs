namespace BlockMaster.Domain.Exceptions.BadRequestException;

public abstract class BadRequestGeneralException : GeneralException
{
    protected BadRequestGeneralException(string? message) : base(message)
    {
    }
}