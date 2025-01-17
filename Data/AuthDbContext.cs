using Microsoft.EntityFrameworkCore;

using LimsAuthenticationService.Models;

namespace LimsAuthenticationService.Data;
public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Define the many-to-many relationship between users and roles
        builder.Entity<UserRole>()
            .HasKey(ur => new { ur.IdUtilisateur, ur.IdRole });

        builder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.IdUtilisateur);

        builder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.IdRole);
    }
    
}
