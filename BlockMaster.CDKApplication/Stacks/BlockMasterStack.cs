using System.Diagnostics.CodeAnalysis;
using Amazon.CDK;
using Amazon.CDK.AWS.ECR;
using IRepository = Amazon.CDK.AWS.ECR.IRepository;
namespace BlockMaster.CDKApplication.Stacks;

[ExcludeFromCodeCoverage]
public class BlockMasterStack : Stack
{
    private IRepository? _repository;

    private void GenerateEcrRepository()
    {
        _repository = new Repository(this, "Block-Master-Ecr", new RepositoryProps());
    }
    
}