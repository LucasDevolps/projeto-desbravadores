using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projeto.desbravadores.Application.Auth;
using projeto.desbravadores.Application.Users;
using projeto.desbravadores.Infrastructure.Auth;
using projeto.desbravadores.Infrastructure.Persistence;
using projeto.desbravadores.Infrastructure.Users;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DesbravadoresDbContext>(
        opt => { opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); }
    );



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bind Jwt options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Jwt Auth
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

// DI do auth
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Migration");
var db = scope.ServiceProvider.GetRequiredService<DesbravadoresDbContext>();

const int maxTries = 10;

for (var attempt = 1; attempt <= maxTries; attempt++)
{
    try
    {
        logger.LogInformation("Applying EF Core migrations (attempt {Attempt}/{Max})...", attempt, maxTries);
        db.Database.Migrate();
        logger.LogInformation("Migrations applied successfully.");
        break;
    }
    catch (SqlException ex)
    {
        logger.LogWarning(ex, "SQL not ready yet. Retrying in 3s...");
        Thread.Sleep(TimeSpan.FromSeconds(3));
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed applying migrations.");
        throw;
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
