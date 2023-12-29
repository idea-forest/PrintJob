using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // No UserName property included here
    public int TeamId { get; set; }
}
