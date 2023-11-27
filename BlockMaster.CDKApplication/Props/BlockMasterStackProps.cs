using System.Diagnostics.CodeAnalysis;
using Amazon.CDK;

namespace BlockMaster.CDKApplication.Props;

[ExcludeFromCodeCoverage]
public class BlockMasterStackProps
{
    public string? ParameterStorePath { get; set; }
    public string? ApiKey { get; set; }
    public Duration? LambdaTimeout { get; set; }
    public string? RedisClusterEndpointName { get; set; }
    public string? CacheSubnetGroupName {get; set; }
}