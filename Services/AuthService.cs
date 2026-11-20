using BackofficeApi.Data;
using BackofficeApi.Dtos;
using BackofficeApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackofficeApi.Services;

public interface IAuthService
{
    Task<LoginRespuestaDto?> LoginAsync(LoginDto dto);
}

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<Usuario> _hasher;
    private readonly ITokenService _tokens;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext db, IPasswordHasher<Usuario> hasher, ITokenService tokens, ILogger<AuthService> logger)
    {
        _db = db;
        _hasher = hasher;
        _tokens = tokens;
        _logger = logger;
    }

    public async Task<LoginRespuestaDto?> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var usuario = await _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

        if (usuario is null)
        {
            _logger.LogWarning("Login fallido para {Email}: el usuario no existe", email);
            return null;
        }

        var resultado = _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password);
        if (resultado == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Login fallido para {Email}: contraseña incorrecta", email);
            return null;
        }

        _logger.LogInformation("Login correcto para el usuario {UsuarioId}", usuario.Id);

        var (token, expira) = _tokens.Generar(usuario);

        return new LoginRespuestaDto
        {
            Token = token,
            Expira = expira,
            Nombre = usuario.Nombre,
            Rol = usuario.Rol
        };
    }
}