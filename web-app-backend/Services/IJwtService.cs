using Microsoft.AspNetCore.Identity;
using ProjectLoc.Dtos.Auth;
using ProjectLoc.Dtos.Auth.Request;
using ProjectLoc.Dtos.Auth.Response;

namespace ProjectLoc.Services;

public interface IJwtService
{
    Task<AuthResult> GenerateToken(ApplicationUser user);
    Task<RefreshTokenResponseDTO> VerifyToken(TokenRequestDTO tokenRequest);

}
