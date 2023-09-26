namespace BlockMaster.Domain.Util;

public static class ExceptionUtil
{
    public const string
        InternalServerErrorMessage = "Action could not be executed",
        MovieNotFoundExceptionMessage = "The movie does not exist",
        MoviesNotFoundExceptionMessage = "No movies found",
        MoviesConflictAlreadyExistMessage = "the movie already exists",
        MovieRequestBadRequestMessage = "Movie request does not meet the requirements",
        MovieRequestCategoryBadRequestMessage = "The inserted category does not correspond to the allowed ones.";
}