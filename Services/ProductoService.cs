using BackofficeApi.Data;
using BackofficeApi.Dtos;
using BackofficeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackofficeApi.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _db;

    public ProductoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductoDto>> ListarAsync(int? categoriaId, int pagina, int tamano)
    {
        var consulta = _db.Productos.AsNoTracking();

        if (categoriaId.HasValue)
        {
            consulta = consulta.Where(p => p.CategoriaId == categoriaId.Value);
        }

        return await consulta
            .OrderBy(p => p.Id)
            .Skip((pagina - 1) * tamano)
            .Take(tamano)
            .Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria!.Nombre
            })
            .ToListAsync();
    }

    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        return await _db.Productos
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria!.Nombre
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductoDto?> CrearAsync(ProductoCrearDto dto)
    {
        var categoriaExiste = await _db.Categorias.AnyAsync(c => c.Id == dto.CategoriaId);
        if (!categoriaExiste)
        {
            return null;
        }

        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Precio = dto.Precio,
            Stock = dto.Stock,
            CategoriaId = dto.CategoriaId
        };

        _db.Productos.Add(producto);
        await _db.SaveChangesAsync();

        return await ObtenerPorIdAsync(producto.Id);
    }
}