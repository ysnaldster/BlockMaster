namespace BlockMaster.Domain.Util;

public static class ConstUtil
{
    public const string
        InternalServerErrorMessage = "Action could not be executed",
        MovieNotFoundExceptionMessage = "The movie does not exist",
        MoviesNotFoundExceptionMessage = "No movies found",
        MoviesConflictAlreadyExistMessage = "the movie already exists",
        MovieRequestBadRequestMessage = "Movie request does not meet the requirements",
        MovieRequestBadRequestMessageByCountry = "The country code provided is not allowed",
        MovieRequestCategoryBadRequestMessage = "The inserted category does not correspond to the allowed ones.";

    public const string
        ParameterStorePath = "/BlockMaster/",
        AwsHost = "http://localhost:4566",
        AwsAccessKey = "123",
        AwsSecretAccessKey = "123";
}