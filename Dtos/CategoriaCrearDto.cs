using System.ComponentModel.DataAnnotations;

namespace BackofficeApi.Dtos;

public class CategoriaCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(80)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Descripcion { get; set; }
}