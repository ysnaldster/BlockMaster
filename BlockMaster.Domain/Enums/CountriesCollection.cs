namespace BlockMaster.Domain.Enums;

public abstract class CountriesCollection
{
    public const string
        Chile = "Chile",
        Colombia = "Colombia",
        Peru = "Perú";

    public enum CountriesPrefix
    {
        Co = 57,
        Pe = 51,
        Cl = 56
    }
}