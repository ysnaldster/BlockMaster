namespace BlockMaster.Domain.Util;

public abstract class ExceptionUtil
{
    public const string
        InternalServerErrorMessage = "Action could not be executed",
        MovieNotFoundExceptionMessage = "The movie does not exist",
        MoviesNotFoundExceptionMessage = "No movies found";
    
}