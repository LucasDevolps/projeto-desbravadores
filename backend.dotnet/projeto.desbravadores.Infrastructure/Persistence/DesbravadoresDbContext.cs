using Microsoft.EntityFrameworkCore;
using projeto.desbravadores.Domain.Auth;
using projeto.desbravadores.Domain.Users;

namespace projeto.desbravadores.Infrastructure.Persistence;

public sealed class DesbravadoresDbContext(
    DbContextOptions<DesbravadoresDbContext> options) 
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(x => x.Email).HasMaxLength(320).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(x => x.TokenHash).HasMaxLength(256).IsRequired();
            entity.HasIndex(x => x.TokenHash).IsUnique();

            entity.HasIndex(entity => entity.UserId);
        });
    }
}
