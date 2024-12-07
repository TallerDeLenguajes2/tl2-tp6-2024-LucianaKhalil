using System.ComponentModel.DataAnnotations;

public class ViewCliente
{   
     [Required(ErrorMessage = " Campo obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
    public string Nombre { get; set; }

    [Phone(ErrorMessage = "El teléfono debe tener un formato válido.")]
    public string Telefono { get; set; }

     [Required(ErrorMessage = "El email es obligatorio.")]
    
    [EmailAddress(ErrorMessage = "Debe ser un email válido.")]
    public string Email { get; set; }

    public int IdCliente { get; set; }

    //constructor
    public ViewCliente(Cliente c)
    {
        Nombre = c.Nombre;
        Telefono = c.Telefono;
        Email = c.Email;
        IdCliente = c.ClienteId;
    }
    
    public ViewCliente()
    {
    }
}
