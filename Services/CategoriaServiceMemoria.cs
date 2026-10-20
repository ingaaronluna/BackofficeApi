using BackofficeApi.Dtos;

namespace BackofficeApi.Services;

public class CategoriaServiceMemoria : ICategoriaService
{
    private readonly List<CategoriaDto> _categorias =
    [
        new() { Id = 1, Nombre = "Electrónica", Descripcion = "Dispositivos y accesorios" },
        new() { Id = 2, Nombre = "Libros", Descripcion = "Material de lectura" }
    ];
    private readonly object _lock = new();
    private int _siguienteId = 3;

    public IEnumerable<CategoriaDto> Listar()
    {
        lock (_lock) { return _categorias.ToList(); }
    }

    public CategoriaDto? ObtenerPorId(int id)
    {
        lock (_lock) { return _categorias.FirstOrDefault(c => c.Id == id); }
    }

    public CategoriaDto Crear(CategoriaCrearDto dto)
    {
        lock (_lock)
        {
            var nueva = new CategoriaDto
            {
                Id = _siguienteId++,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };
            _categorias.Add(nueva);
            return nueva;
        }
    }
}