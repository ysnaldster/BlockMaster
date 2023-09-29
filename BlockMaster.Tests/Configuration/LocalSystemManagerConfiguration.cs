using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using BlockMaster.Tests.Util;

namespace BlockMaster.Tests.Configuration;

public class LocalSystemManagerConfiguration
{
    private static readonly AmazonSimpleSystemsManagementConfig SystemsManagementConfig = new()
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(ConstUtil.AwsRegion),
        ServiceURL = ConstUtil.AwsHost
    };

    private static readonly AmazonSimpleSystemsManagementClient SystemsManagementClient =
        new(ConstUtil.AwsAccessKey, ConstUtil.AwsSecretAccessKey, SystemsManagementConfig);

    public static async Task ConfigureParameterStore()
    {
        await ConfigureParameter("/BlockMaster/DynamoDbMoviesTableName", ConstUtil.MovieTableName);
    }

    private static async Task ConfigureParameter(string name, string value)
    {
        var putParameterRequest = new PutParameterRequest
        {
            Name = name,
            Value = value,
            Type = ParameterType.String,
            DataType = "text",
            Tier = ParameterTier.Standard
        };

        await SystemsManagementClient.PutParameterAsync(putParameterRequest);
    }
}