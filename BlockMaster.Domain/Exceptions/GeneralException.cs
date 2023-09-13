namespace BlockMaster.Domain.Exceptions;

public class GeneralException : Exception
{
    protected GeneralException(string? message) : base(message)
    {
    }
}