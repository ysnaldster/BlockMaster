using BlockMaster.Infrastructure.Clients;

namespace BlockMaster.Tests.Clients;

public class TestElastiCacheClient : ElastiCacheClient
{
    public TestElastiCacheClient(string endpoint) : base(endpoint)
    {
    }
}