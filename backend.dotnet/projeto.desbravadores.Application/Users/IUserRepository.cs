using projeto.desbravadores.Domain.Users;

namespace projeto.desbravadores.Application.Users;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}
