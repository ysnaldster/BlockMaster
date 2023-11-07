using System.Diagnostics.CodeAnalysis;
using Amazon.CDK;
using BlockMaster.CDKApplication.Props;
using BlockMaster.CDKApplication.Stacks;

namespace BlockMaster.CDKApplication;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main()
    {
        var app = new App();
        var blockMasterStackProps = new BlockMasterStackProps
        {
            ParameterStorePath = app.Node.TryGetContext("ParameterStorePath") as string,
            ApiKey = app.Node.TryGetContext("ApiKey") as string
        };
        _ = new BlockMasterStack(app, "BlockMasterCdkApplicationStack", new StackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = "757892324335", //Here is the value of the account
                Region = "us-east-1",
            }
        }, blockMasterStackProps);

        app.Synth();
    }
}