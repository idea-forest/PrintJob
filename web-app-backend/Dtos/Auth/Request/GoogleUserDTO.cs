using System.ComponentModel.DataAnnotations;

namespace ProjectLoc.Dtos.Auth.Request;

public class GoogleUserDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
