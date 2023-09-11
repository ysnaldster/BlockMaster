using System.Diagnostics.CodeAnalysis;
using Amazon;
using Amazon.DynamoDBv2;
using Autofac;

namespace BlockMaster.Api.Util.Modules;

[ExcludeFromCodeCoverage]
public class ClientsModule : Module
{
    #region protected methods

    protected override void Load(ContainerBuilder builder)
    {
        builder
            .Register((_, _) => new AmazonDynamoDBClient(RegionEndpoint.USEast1))
            .As<IAmazonDynamoDB>()
            .SingleInstance();
    }

    #endregion
}