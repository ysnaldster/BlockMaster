namespace BlockMaster.Domain.Request.Identity;

public class TokenGenerationRequest
{
    public string? UserId { get; set; }
    public string? Email { get; set; }
    
    public Dictionary<string, object>? CustomClaims { get; set; }

}