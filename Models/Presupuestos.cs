
public class Presupuesto{
    private int idPresupuesto;
    private string nombreDestinatario;

    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle = new List<PresupuestoDetalle>();//inicializo la lista

    private const double IVA=0.21;
    
    //-------------------------
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }

    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

    //constructor presupuesto
    public Presupuesto(int idPresupuesto, string nombreDestinatario){
        this.IdPresupuesto = idPresupuesto;
        this.NombreDestinatario = nombreDestinatario;
        this.FechaCreacion = DateTime.Now;
    }
    public Presupuesto(){//constructor por defecto
    }
    /* agregar IVA MontoPresupuesto ()
■ MontoPresupuestoConIva()
■ CantidadProductos ()*/
//monto sin IVA
    public double MontoPresupuesto(){ 
        double montoInicial=0;
        foreach(var presupuesto in Detalle){
            montoInicial+=presupuesto.Producto.Precio*presupuesto.Cantidad;
        }
        return montoInicial;
        }
    public double MontoPresupuestoConIva(){
        double montoIva=0;
        foreach(var presupuesto in Detalle){
            montoIva+=((presupuesto.Producto.Precio)*1.21)*presupuesto.Cantidad;
        }
         return montoIva;
        }
       
    public int CantidadProductos (){
        int cantidad=0;
        foreach(var producto in Detalle){
            cantidad+=producto.Cantidad;
        }
        return cantidad;

    }
}