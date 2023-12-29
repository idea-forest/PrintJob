using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectLoc.Models;
using ProjectLoc.Services;

namespace ProjectLoc.Data;

public class ApiDbContext : IdentityDbContext<ApplicationUser>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> opt) : base(opt)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().ToTable("Users");
    }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Team> Teams { get; set; }
    public virtual DbSet<Device> Devices { get; set; }
    public virtual DbSet<Printer> Printers { get; set; }
    public virtual DbSet<PrintJob> PrintJobs { get; set; }
}
