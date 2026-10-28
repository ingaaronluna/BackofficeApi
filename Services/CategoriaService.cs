using BackofficeApi.Data;
using BackofficeApi.Dtos;
using BackofficeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackofficeApi.Services;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _db;

    public CategoriaService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CategoriaDto>> ListarAsync()
    {
        return await _db.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            })
            .ToListAsync();
    }

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
    {
        return await _db.Categorias
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoriaDto> CrearAsync(CategoriaCrearDto dto)
    {
        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        _db.Categorias.Add(categoria);
        await _db.SaveChangesAsync();

        return new CategoriaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };
    }

    public async Task<bool> ActualizarAsync(int id, CategoriaCrearDto dto)
    {
        var categoria = await _db.Categorias.FindAsync(id);
        if (categoria is null)
        {
            return false;
        }

        categoria.Nombre = dto.Nombre;
        categoria.Descripcion = dto.Descripcion;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<ResultadoEliminacion> EliminarAsync(int id)
    {
        var categoria = await _db.Categorias.FindAsync(id);
        if (categoria is null)
        {
            return ResultadoEliminacion.NoEncontrada;
        }

        var tieneProductos = await _db.Productos.AnyAsync(p => p.CategoriaId == id);
        if (tieneProductos)
        {
            return ResultadoEliminacion.TieneProductos;
        }

        _db.Categorias.Remove(categoria);
        await _db.SaveChangesAsync();
        return ResultadoEliminacion.Eliminada;
    }
}