using BackofficeApi.Data;
using BackofficeApi.Dtos;
using BackofficeApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackofficeApi.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<Usuario> _hasher;

    private readonly ICifradoService _cifrado;

    public UsuarioService(AppDbContext db, IPasswordHasher<Usuario> hasher, ICifradoService cifrado)
    {
        _db = db;
        _hasher = hasher;
        _cifrado = cifrado;
    }

    public async Task<IEnumerable<UsuarioDto>> ListarAsync()
    {
        var usuarios = await _db.Usuarios.AsNoTracking().OrderBy(u => u.Id).ToListAsync();
        return usuarios.Select(ADto).ToList();
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
    {
        var usuario = await _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        return usuario is null ? null : ADto(usuario);
    }

    public async Task<UsuarioDto?> CrearAsync(UsuarioCrearDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        if (await _db.Usuarios.AnyAsync(u => u.Email == email))
        {
            return null;
        }

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = email
        };

        usuario.PasswordHash = _hasher.HashPassword(usuario, dto.Password);
        if (!string.IsNullOrWhiteSpace(dto.Telefono))
        {
            usuario.Telefono = _cifrado.Cifrar(dto.Telefono);
        }

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();
        return ADto(usuario);
    }

    private UsuarioDto ADto(Usuario u)
    {
        return new UsuarioDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Email = u.Email,
            Telefono = u.Telefono is null ? null : _cifrado.Descifrar(u.Telefono),
            Rol = u.Rol,
            FechaCreacion = u.FechaCreacion
        };
    }
}