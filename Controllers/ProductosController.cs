using BackofficeApi.Dtos;
using BackofficeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackofficeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductosController(IProductoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Listar(
        [FromQuery] int? categoriaId,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamano = 10)
    {
        if (pagina < 1 || tamano < 1 || tamano > 50)
        {
            return BadRequest(new { mensaje = "Paginación inválida" });
        }

        return Ok(await _service.ListarAsync(categoriaId, pagina, tamano));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> ObtenerPorId(int id)
    {
        var producto = await _service.ObtenerPorIdAsync(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Crear(ProductoCrearDto dto)
    {
        var creado = await _service.CrearAsync(dto);
        if (creado is null)
        {
            return BadRequest(new { mensaje = "La categoría indicada no existe" });
        }

        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }
}