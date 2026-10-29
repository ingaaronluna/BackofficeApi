using BackofficeApi.Dtos;

namespace BackofficeApi.Services;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> ListarAsync(int? categoriaId, int pagina, int tamano);
    Task<ProductoDto?> ObtenerPorIdAsync(int id);
    Task<ProductoDto?> CrearAsync(ProductoCrearDto dto);
}