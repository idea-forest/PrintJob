using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectLoc.Models;

public class Team
{
    public int Id { get; set; }
    public string? UserName { get; set; }
}