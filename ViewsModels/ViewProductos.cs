using System.ComponentModel.DataAnnotations;

public class ViewProductos
{
    public int IdProducto { get ; set;}
    
    [StringLength(250 , ErrorMessage = "La descripcion no puede tener mas de 250 caracteres")] 
    public string Descripcion { get; set; }
       [Range(1,int.MaxValue , ErrorMessage ="El precio debe ser mayor a 0")]  [Required(ErrorMessage ="Este campo es obligatorio")]
    public int Precio { get; set; }

    //constructor
    public ViewProductos(Productos Prod){
        IdProducto=Prod.IdProducto;
        Descripcion=Prod.Descripcion;
        Precio=Prod.Precio;
    }
     public ViewProductos()
    {
    }
}

