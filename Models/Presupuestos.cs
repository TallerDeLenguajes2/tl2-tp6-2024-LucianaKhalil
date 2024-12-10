
public class Presupuesto{
    private int idPresupuesto;
    private Cliente cliente;

    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle = new List<PresupuestoDetalle>();//inicializo la lista

    private const double IVA=0.21;
    
    //-------------------------
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }


    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }

    //constructor presupuesto
    
    public Presupuesto(int idPresupuesto , Cliente cliente , List<PresupuestoDetalle> detalles , DateTime FechaCreacion )
    {
        this.idPresupuesto = idPresupuesto ; 
        this.cliente = cliente ; 
        this.detalle = detalles;
        this.FechaCreacion = FechaCreacion ;
    }
    public Presupuesto(int idPresupuesto, Cliente cliente){
        this.IdPresupuesto = idPresupuesto;
        this.Cliente = cliente;
        this.FechaCreacion = DateTime.Now;
    }
     public Presupuesto()
    {
        detalle = new List<PresupuestoDetalle>();
        cliente = new Cliente();
    }

    //crear constructor viewmodel
    public Presupuesto(ViewAltaPresupuesto viewAltaPresupuestoVm){
        cliente = new Cliente();
        cliente.ClienteId = viewAltaPresupuestoVm.ClienteId;
        FechaCreacion = viewAltaPresupuestoVm.Fecha ;
    }
    //constructor para modificar
     public Presupuesto(ViewEditarPresupuesto presupuestoVM)
    {
        idPresupuesto = presupuestoVM.IdPresupuesto;
        cliente = new Cliente();
        cliente.ClienteId = presupuestoVM.ClienteId;
        fechaCreacion = presupuestoVM.Fecha;
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
            montoIva+=(presupuesto.Producto.Precio)*1.21*presupuesto.Cantidad;
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