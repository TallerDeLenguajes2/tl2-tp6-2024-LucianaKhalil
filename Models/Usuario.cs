using Microsoft.AspNetCore.Identity;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string NombreUsuario { get; set; }
    public string  Password { get; set; }

    public Rol RolUsuario {get; set; }
}

public enum Rol
{
    Admin,
    Cliente,
    noLogueado
}