using projeto.desbravadores.Application.Auth;
using projeto.desbravadores.Application.Users;
using projeto.desbravadores.Domain.Auth;

namespace projeto.desbravadores.Infrastructure.Auth;

public sealed class AuthService(IUserRepository userRepository,
    ITokenService tokenService,
    IRefreshTokenRepository refreshTokenRepository)
         : IAuthService 
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    
    public async Task<TokenResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        bool ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!ok)
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        var userData = new UserTokenData(UserId: user.Id, Email: user.Email, DisplayName: user.DisplayName, Roles: user.Roles);

        var tokens = _tokenService.GenerateTokens(userData);

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = TokenHashing.Sha256(tokens.RefreshToken),
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = tokens.RefreshTokenExpiresAt
        };


        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        
        return tokens;
    }

    public async Task LogoutAsync(RefreshRequest request, CancellationToken cancellationToken)
    {
        string incomingHash = TokenHashing.Sha256(request.refreshToken);

        var stored = await _refreshTokenRepository.GetByTokenHashAsync(incomingHash, cancellationToken);
        if (stored is null) return;

        stored.RevokedAtUtc = DateTime.UtcNow;
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<TokenResponse> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken)
    {
        string incomingHash = TokenHashing.Sha256(request.refreshToken);

        var stored = await _refreshTokenRepository.GetByTokenHashAsync(incomingHash, cancellationToken);
        if (stored is null || !stored.IsActive)
            throw new UnauthorizedAccessException("Token de atualização inválido.");

        var user = await _userRepository.GetByIdAsync(stored.UserId, cancellationToken);
        if (user is null)
            throw new UnauthorizedAccessException("Usuário não encontrado.");

        var userData = new UserTokenData(UserId: user.Id, Email: user.Email, DisplayName: user.DisplayName, Roles: user.Roles);
        var newTokens = _tokenService.GenerateTokens(userData);

        stored.RevokedAtUtc = DateTime.UtcNow;
        stored.ReplacedByTokenHash = TokenHashing.Sha256(newTokens.RefreshToken);

        var newEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = stored.ReplacedByTokenHash,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = newTokens.RefreshTokenExpiresAt
        };

        await _refreshTokenRepository.AddAsync(newEntity, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return newTokens;
    }
}
