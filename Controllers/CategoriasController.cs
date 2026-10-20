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
    public ActionResult<IEnumerable<CategoriaDto>> Listar()
    {
        return Ok(_service.Listar());
    }

    [HttpGet("{id:int}")]
    public ActionResult<CategoriaDto> ObtenerPorId(int id)
    {
        var categoria = _service.ObtenerPorId(id);
        return categoria is null ? NotFound() : Ok(categoria);
    }

    [HttpPost]
    public ActionResult<CategoriaDto> Crear(CategoriaCrearDto dto)
    {
        var creada = _service.Crear(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }
}