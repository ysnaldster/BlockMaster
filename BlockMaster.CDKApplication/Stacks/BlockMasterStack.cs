using System.Diagnostics.CodeAnalysis;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.ElastiCache;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SSM;
using BlockMaster.CDKApplication.Props;
using IRepository = Amazon.CDK.AWS.ECR.IRepository;
using Attribute = Amazon.CDK.AWS.DynamoDB.Attribute;
using Construct = Constructs.Construct;
using TagsManager = Amazon.CDK.Tags;

namespace BlockMaster.CDKApplication.Stacks;

[ExcludeFromCodeCoverage]
public class BlockMasterStack : Stack
{
    private string? _dynamoDbTableName;
    private IManagedPolicy? _managedPolicy;
    private IRepository? _repository;
    private Function? _blockMasterLambda;
    private IVpc? _vpc;
    private readonly Dictionary<string, ISecurityGroup> _securityGroups = new();

    public BlockMasterStack(Construct scope, string id, IStackProps props, BlockMasterStackProps customProps) :
        base(scope, id, props)
    {
        GenerateManagementPolicy();
        GenerateEcrRepository();
        CreateDynamoTable();
        CreateNetwork(customProps);
        CreateElastiCacheCluster(customProps);
        GenerateLambda(customProps, true);
        AttachApiGatewayToLambda();
        AddParameterStore(customProps);
    }

    private void GenerateEcrRepository()
    {
        _repository = new Repository(this, "block-master-ecr", new RepositoryProps());
    }

    private void GenerateLambda(BlockMasterStackProps props, bool isImageCreated = false)
    {
        if (!isImageCreated) return;
        _blockMasterLambda = new Function(this, "block-master-lambda", new FunctionProps
        {
            Code = new EcrImageCode(_repository!),
            Handler = Handler.FROM_IMAGE,
            Runtime = Runtime.FROM_IMAGE,
            Environment = new Dictionary<string, string> { { "PARAMETER_STORE_PATH", props.ParameterStorePath! } },
            Timeout = props.LambdaTimeout,
            Vpc = _vpc,
            SecurityGroups = new[] { _securityGroups["BlockMasterRedisSecurityGroup"] },
        });
        _blockMasterLambda.Role!.AddManagedPolicy(_managedPolicy!);
        TagsManager.Of(_blockMasterLambda).Add("Name", "BlockMasterLambda");
    }

    private void AttachApiGatewayToLambda()
    {
        var apiGateway = new RestApi(this, "BlockMasterApiGateway");
        var integrationWithLambda = new LambdaIntegration(_blockMasterLambda!);
        apiGateway.Root.AddMethod("ANY", integrationWithLambda);
    }

    private void CreateNetwork(BlockMasterStackProps customProps)
    {
        _vpc = new Vpc(this, "BlockMasterVpc", new VpcProps
        {
            MaxAzs = 2,
            SubnetConfiguration = new ISubnetConfiguration[]
            {
                new SubnetConfiguration
                {
                    SubnetType = SubnetType.PUBLIC,
                    Name = "PublicSubnet",
                    CidrMask = 24,
                },
                new SubnetConfiguration
                {
                    SubnetType = SubnetType.PRIVATE_ISOLATED,
                    Name = "PrivateSubnet",
                    CidrMask = 24,
                },
            },
        });

        _securityGroups["BlockMasterRedisSecurityGroup"] = new SecurityGroup(this, "BlockMasterRedisSecurityGroup",
            new SecurityGroupProps
            {
                Vpc = _vpc
            });
        var publicSubnetIds = _vpc.SelectSubnets(new SubnetSelection { SubnetType = SubnetType.PUBLIC })
            .Subnets
            .Select(subnet => subnet.SubnetId)
            .ToArray();

        _ = new CfnSubnetGroup(this, "MyCacheSubnetGroup", new CfnSubnetGroupProps
        {
            CacheSubnetGroupName = customProps.CacheSubnetGroupName,
            SubnetIds = publicSubnetIds,
            Description = "Block Master Subnet Group",
        });
    }


    private void CreateElastiCacheCluster(BlockMasterStackProps customProps)
    {
        var elasticacheCluster = new CfnCacheCluster(this, "BlockMasterRedisCluster", new CfnCacheClusterProps
        {
            CacheNodeType = "cache.t2.micro",
            Engine = "redis",
            NumCacheNodes = 1,
            VpcSecurityGroupIds = new[] { _securityGroups["BlockMasterRedisSecurityGroup"].SecurityGroupId },
            CacheSubnetGroupName = customProps.CacheSubnetGroupName,
        });
        customProps.RedisClusterEndpointName = elasticacheCluster.AttrRedisEndpointAddress;
    }

    private void CreateDynamoTable()
    {
        var healthCheckTable = new Table(this, "Movies", new TableProps()
        {
            BillingMode = BillingMode.PAY_PER_REQUEST,
            PartitionKey = new Attribute { Name = "Id", Type = AttributeType.NUMBER }
        });
        _dynamoDbTableName = healthCheckTable.TableName;
        TagsManager.Of(healthCheckTable).Add("Name", "MoviesTable");
    }

    private void AddParameterStore(BlockMasterStackProps props)
    {
        var parameters = new Dictionary<string, string>
        {
            { $"{props.ParameterStorePath}/DynamoDbMoviesTableName", _dynamoDbTableName! },
            { $"{props.ParameterStorePath}/ApiKey", props.ApiKey! },
            { $"{props.ParameterStorePath}/RedisEndpoint", props.RedisClusterEndpointName! }
        };

        foreach (var parameter in parameters)
        {
            _ = new StringParameter(this, parameter.Key, new StringParameterProps
            {
                ParameterName = parameter.Key,
                StringValue = parameter.Value,
                Tier = ParameterTier.STANDARD
            });
        }
    }

    private void GenerateManagementPolicy()
    {
        _managedPolicy = new ManagedPolicy(this, "BlockMasterPolicy", new ManagedPolicyProps
        {
            Statements = new[]
            {
                GenerateSystemManagerPolicyStatement(),
                GenerateDynamoPolicyStatement(),
                GenerateElastiCacheRedisPolicyStatement()
            }
        });
    }

    private static PolicyStatement GenerateDynamoPolicyStatement()
    {
        return new PolicyStatement(new PolicyStatementProps()
        {
            Effect = Effect.ALLOW,
            Resources = new[]
            {
                "*"
            },
            Actions = new[]
            {
                "dynamodb:PutItem",
                "dynamodb:DescribeTable",
                "dynamodb:DeleteItem",
                "dynamodb:GetItem",
                "dynamodb:UpdateItem",
                "dynamodb:BatchGetItem",
                "dynamodb:BatchWriteItem",
                "dynamodb:Scan",
                "dynamodb:Query"
            }
        });
    }

    private static PolicyStatement GenerateElastiCacheRedisPolicyStatement()
    {
        return new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Resources = new[] { "*" },
            Actions = new[]
            {
                "elasticache:DescribeCacheClusters",
                "elasticache:ListTagsForResource",
                "elasticache:DescribeReplicationGroups",
                "elasticache:DescribeCacheParameters",
                "elasticache:DescribeCacheSecurityGroups",
                "elasticache:DescribeCacheSubnetGroups"
            }
        });
    }

    private static PolicyStatement GenerateSystemManagerPolicyStatement()
    {
        return new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Resources = new[]
            {
                "*"
            },
            Actions = new[]
            {
                "ssm:DescribeParameters",
                "ssm:GetParametersByPath",
                "ssm:GetParameters",
                "ssm:GetParameter"
            }
        });
    }
}