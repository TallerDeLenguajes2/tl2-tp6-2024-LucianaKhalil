using System.ComponentModel.DataAnnotations;


public class ViewAltaPresupuesto
{

    public int ClienteId { get; set; }
    
    public DateTime Fecha { get; set; }
    public List<Cliente> clientes { get; set; }
    public ViewAltaPresupuesto()
    {
        clientes = new List<Cliente>();
    }
}
public class viewAgregarProductoAlPresupuesto
{
    public int IdPresupuesto { get; set; }
    public int IdProducto { get; set; }

    public List<Productos> productos { get; set; }
    public int cantidad { get; set; }
    //Para Alta de producto a presupuesto: Incluir el listado de los productos para que se pueda seleccionar entre ellos.
    public viewAgregarProductoAlPresupuesto()
    {
        productos = new List<Productos>() ;
    }
}

public class ViewEditarPresupuesto
{
    public int IdPresupuesto { get; set; } // ID del presupuesto
    public int ClienteId { get; set; } // ID del cliente
    public DateTime Fecha { get; set; } // Fecha de creación del presupuesto
     public ViewEditarPresupuesto()
    {

    }
}
