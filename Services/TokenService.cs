using System.Security.Claims;
using System.Text;
using BackofficeApi.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BackofficeApi.Services;

public interface ITokenService
{
    (string Token, DateTime Expira) Generar(Usuario usuario);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuracion;

    public TokenService(IConfiguration configuracion)
    {
        _configuracion = configuracion;
    }

    public (string Token, DateTime Expira) Generar(Usuario usuario)
    {
        var clave = _configuracion["Jwt:Clave"]
            ?? throw new InvalidOperationException("Falta el secreto Jwt:Clave");

        var minutos = _configuracion.GetValue<int?>("Jwt:MinutosExpiracion") ?? 30;
        var expira = DateTime.UtcNow.AddMinutes(minutos);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("name", usuario.Nombre),
                new Claim("role", usuario.Rol)
            }),
            Expires = expira,
            Issuer = _configuracion["Jwt:Emisor"],
            Audience = _configuracion["Jwt:Audiencia"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave)),
                SecurityAlgorithms.HmacSha256)
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);
        return (token, expira);
    }
}