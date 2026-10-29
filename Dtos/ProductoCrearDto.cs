using System.ComponentModel.DataAnnotations;

namespace BackofficeApi.Dtos;

public class ProductoCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor que cero")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Indique una categoría válida")]
    public int CategoriaId { get; set; }
}