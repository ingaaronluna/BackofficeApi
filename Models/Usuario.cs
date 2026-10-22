namespace BackofficeApi.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string Rol { get; set; } = "Administrador";
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}