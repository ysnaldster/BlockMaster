using BlockMaster.Domain.Request.Identity;

namespace BlockMaster.Domain.Services;

public interface IGenerateTokenService
{
    string GenerateToken(TokenGenerationRequest request);
}