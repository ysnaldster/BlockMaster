﻿using System.Globalization;
using BlockMaster.Domain.Enums;
using BlockMaster.Domain.Exceptions.BadRequestException;
using BlockMaster.Domain.Util;

namespace BlockMaster.Business.Util;

public static class CountryEvaluator
{
    public static string ConvertCountryCodeToCountryName(long countryName)
    {
        var countryNameToString = countryName.ToString();
        var countryToParse =
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(countryNameToString?.ToLower() ?? string.Empty);
        var country = Enum.Parse<CountriesCollection.CountriesPrefix>
            (countryToParse ?? throw new ArgumentNullException(nameof(countryName)));
        var validate = country switch
        {
            CountriesCollection.CountriesPrefix.Pe => CountriesCollection.Peru,
            CountriesCollection.CountriesPrefix.Co => CountriesCollection.Colombia,
            CountriesCollection.CountriesPrefix.Cl => CountriesCollection.Chile,
            _ => throw new MovieRequestBadRequestException(ConstUtil.MovieRequestBadRequestMessageByCountry)
        };

        return validate;
    }
}