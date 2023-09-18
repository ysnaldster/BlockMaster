﻿namespace BlockMaster.Domain.Request;

public class MovieRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CountryCode { get; set; }
    public double? Score { get; set; }
    public string? Category { get; set; }
}