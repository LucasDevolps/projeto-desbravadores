using Microsoft.EntityFrameworkCore;
using projeto.desbravadores.Application.Auth;
using projeto.desbravadores.Domain.Auth;
using projeto.desbravadores.Infrastructure.Persistence;

namespace projeto.desbravadores.Infrastructure.Auth;

public sealed class RefreshTokenRepository
    (DesbravadoresDbContext context) : IRefreshTokenRepository
{
    public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        => context.RefreshTokens.AddAsync(refreshToken, cancellationToken).AsTask();

    public Task<RefreshToken?> GetByTokenHashAsync(string token, CancellationToken cancellationToken)
        => context.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == token, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => context.SaveChangesAsync(cancellationToken);
}
