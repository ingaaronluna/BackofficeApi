using BackofficeApi.Dtos;

namespace BackofficeApi.Services;

public enum ResultadoEliminacion
{
    Eliminada,
    NoEncontrada,
    TieneProductos
}

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> ListarAsync();
    Task<CategoriaDto?> ObtenerPorIdAsync(int id);
    Task<CategoriaDto> CrearAsync(CategoriaCrearDto dto);
    Task<bool> ActualizarAsync(int id, CategoriaCrearDto dto);
    Task<ResultadoEliminacion> EliminarAsync(int id);
}