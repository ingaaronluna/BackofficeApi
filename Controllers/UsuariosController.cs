using BackofficeApi.Dtos;
using BackofficeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackofficeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> Listar()
    {
        return Ok(await _service.ListarAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioDto>> ObtenerPorId(int id)
    {
        var usuario = await _service.ObtenerPorIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Crear(UsuarioCrearDto dto)
    {
        var creado = await _service.CrearAsync(dto);
        if (creado is null)
        {
            return Conflict(new { mensaje = "Ya existe un usuario con ese correo" });
        }

        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }
}