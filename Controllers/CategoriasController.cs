using BackofficeApi.Dtos;
using BackofficeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackofficeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> Listar()
    {
        return Ok(await _service.ListarAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> ObtenerPorId(int id)
    {
        var categoria = await _service.ObtenerPorIdAsync(id);
        return categoria is null ? NotFound() : Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Crear(CategoriaCrearDto dto)
    {
        var creada = await _service.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, CategoriaCrearDto dto)
    {
        var actualizada = await _service.ActualizarAsync(id, dto);
        return actualizada ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await _service.EliminarAsync(id);
        return resultado switch
        {
            ResultadoEliminacion.Eliminada => NoContent(),
            ResultadoEliminacion.TieneProductos => Conflict(new { mensaje = "La categoría tiene productos asociados" }),
            _ => NotFound()
        };
    }
}