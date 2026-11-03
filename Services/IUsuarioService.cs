using BackofficeApi.Dtos;

namespace BackofficeApi.Services;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ListarAsync();
    Task<UsuarioDto?> ObtenerPorIdAsync(int id);
    Task<UsuarioDto?> CrearAsync(UsuarioCrearDto dto);
}