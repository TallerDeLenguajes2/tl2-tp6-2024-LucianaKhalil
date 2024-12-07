using System.ComponentModel.DataAnnotations;


public class ViewAltaPresupuesto
{

    public int idCliente { get; set; }
    
    public DateTime Fecha { get; set; }
    public List<Cliente> clientes { get; set; }
    public ViewAltaPresupuesto()
    {
        clientes = new List<Cliente>();
    }
}
public class viewAgregarProductoAlPresupuesto
{


    public int idPre { get; set; }
    public int idPro { get; set; }

    public List<Productos> productos { get; set; }
    public int cantidad { get; set; }
    //Para Alta de producto a presupuesto: Incluir el listado de los productos para que se pueda seleccionar entre ellos.
    public viewAgregarProductoAlPresupuesto()
    {
        productos = new List<Productos>() ;
    }
}