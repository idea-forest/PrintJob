namespace ProjectLoc.Dtos.Auth;

public class AuthResult
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public bool Success { get; set; }
    public ApplicationUser? User { get; set; }
    public List<string>? Errors { get; set; }
}
