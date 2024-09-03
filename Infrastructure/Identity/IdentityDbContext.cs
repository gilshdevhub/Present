using Core.Entities.Identity;
using Core.Entities.PagesManagement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Identity;

public class IdentityDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public DbSet<Page> Page { get; set; }
    public DbSet<PageRoleNew> PageRoleNew { get; set; }
 
    protected readonly IConfiguration _configuration;
    public IdentityDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
               options.UseSqlServer(_configuration.GetConnectionString("rail"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<PageRoleNew>(pageRole =>
        {
            _ = pageRole.HasKey(p => new { p.RoleId, p.PageId });

            _ = pageRole.HasOne(pr => pr.Page)
                .WithMany(r => r.PageRoleNew)
                .HasForeignKey(ur => ur.PageId);

            _ = pageRole.HasOne(ur => ur.Role)
                .WithMany(r => r.PageRoleNew)
                .HasForeignKey(ur => ur.RoleId);
        });
    }
}
