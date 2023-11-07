﻿using System.Diagnostics.CodeAnalysis;
using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.SSM;
using BlockMaster.CDKApplication.Props;
using IRepository = Amazon.CDK.AWS.ECR.IRepository;
using Attribute = Amazon.CDK.AWS.DynamoDB.Attribute;
using Construct = Constructs.Construct;

namespace BlockMaster.CDKApplication.Stacks;

using TagsManager = Tags;

[ExcludeFromCodeCoverage]
public class BlockMasterStack : Stack
{
    private string? _dynamoDbTableName;
    private IManagedPolicy? _managedPolicy;
    private IRepository? _repository;

    public BlockMasterStack(Construct scope, string id, IStackProps props, BlockMasterStackProps customProps) :
        base(scope, id, props)
    {
        GenerateManagementPolicy();
        GenerateEcrRepository();
        CreateDynamoTable();
        AddParameterStore(customProps);
    }

    private void GenerateEcrRepository()
    {
        _repository = new Repository(this, "block-master-ecr", new RepositoryProps());
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
            { $"{props.ParameterStorePath}/ApiKey", props.ApiKey! }
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