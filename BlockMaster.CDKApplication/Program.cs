using System.Diagnostics.CodeAnalysis;
using Amazon.CDK;
using BlockMaster.CDKApplication.Props;
using BlockMaster.CDKApplication.Stacks;

namespace BlockMaster.CDKApplication;

[ExcludeFromCodeCoverage]
public static class Program
{
    #region private methods

    public static void Main(string[] args)
    {
        var app = new App();
        var blockMasterStackProps = new BlockMasterStackProps()
        {
            ParameterStorePath = app.Node.TryGetContext("ParameterStorePath") as string,
            MoviesTableName = app.Node.TryGetContext("MoviesTableName") as string
        };
        _ = new BlockMasterStack(app, "BlockMasterCdkApplicationStack", new StackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = "", //Here is the value of the account
                Region = "us-east-1",
            }
        }, blockMasterStackProps);
        app.Synth();
    }

    #endregion
}