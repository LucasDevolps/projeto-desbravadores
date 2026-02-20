using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using projeto.desbravadores.Application.Auth;

namespace projeto.desbravadores.Infrastructure.Auth;

public sealed class TokenService(IOptions<JwtOptions> options)
        : ITokenService
{
    private readonly JwtOptions _jwt = options.Value;

    public TokenResponse GenerateTokens(UserTokenData user)
    {
        DateTime now = DateTime.UtcNow;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email),
            new (JwtRegisteredClaimNames.UniqueName, user.DisplayName)
        };

        foreach (string role in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        DateTime accessExpires = now.AddMinutes(_jwt.AccessTokenMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: now,
            expires: accessExpires,
            signingCredentials: creds
        );

        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        string refreshToken = GenerateTokens();
        DateTime refreshExpires = now.AddDays(_jwt.RefreshTokenDays);

        return new TokenResponse(accessToken, accessExpires, refreshToken, refreshExpires);
    }
    private static string GenerateTokens()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
    private static string GenerateRefreshTokenPlain()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
// Helper criado para gerar o hash do token de refresh, caso queira armazenar o hash no banco de dados para validação futura
public sealed class TokenHashing
{
    public static string Sha256(string input)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}

