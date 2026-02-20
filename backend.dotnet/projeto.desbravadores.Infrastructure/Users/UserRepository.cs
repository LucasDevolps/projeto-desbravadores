using Microsoft.EntityFrameworkCore;
using projeto.desbravadores.Application.Users;
using projeto.desbravadores.Domain.Users;
using projeto.desbravadores.Infrastructure.Persistence;

namespace projeto.desbravadores.Infrastructure.Users;

public class UserRepository(
    DesbravadoresDbContext context
    ) : IUserRepository
{
    public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        => context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        => context.Users.FindAsync(userId, cancellationToken).AsTask();
}
