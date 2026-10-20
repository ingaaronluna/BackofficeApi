using BackofficeApi.Dtos;

namespace BackofficeApi.Services;

public interface ICategoriaService
{
    IEnumerable<CategoriaDto> Listar();
    CategoriaDto? ObtenerPorId(int id);
    CategoriaDto Crear(CategoriaCrearDto dto);
}