using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_LucianaKhalil.Models;

namespace tl2_tp6_2024_LucianaKhalil.Controllers
{
    public class PresupuestoController : Controller
    {
        private readonly PresupuestoRepositorio _presupuestoRepositorio;
        private readonly ProductoRepositorio _productoRepositorio; // Aquí agregamos el repositorio de productos

        private readonly ClienteRepositorio _clienteRepositorio;

    // Constructor
    public PresupuestoController()
    {
        _presupuestoRepositorio = new PresupuestoRepositorio(@"Data Source=db\Tienda.db;Cache=Shared");
        _productoRepositorio = new ProductoRepositorio(@"Data Source=db\Tienda.db;Cache=Shared"); // Iniciamos el repositorio de productos
        _clienteRepositorio= new ClienteRepositorio(@"Data Source=db\Tienda.db;Cache=Shared");//
    }
    
        //CREAR PRESUPUESTO----------------------------------------------
        [HttpGet]
        public IActionResult CrearPresupuesto()
        {
            var model = new ViewAltaPresupuesto
            {
                clientes = _clienteRepositorio.getAll() 
            };
            return View(model); 
        }

        [HttpPost]
        public IActionResult CrearPresupuesto(ViewAltaPresupuesto altapresupuestoVM)
        {   
            var p=new Presupuesto(altapresupuestoVM);
            _presupuestoRepositorio.Create(p);
            return RedirectToAction("ListarPresupuesto"); 
        }
        //EDITAR PRESUPUESTO---------------------------------------------
        [HttpGet]
        public IActionResult EditarPresupuesto(int idPresupuesto)
        {
            var clientes = _clienteRepositorio.getAll();

            var presupuesto = _presupuestoRepositorio.GetById(idPresupuesto);
            var presupuestoVM = new ViewEditarPresupuesto
            {
                IdPresupuesto = idPresupuesto,
                ClienteId = presupuesto.Cliente.ClienteId,
                Fecha = presupuesto.FechaCreacion
            };

            ViewData["Clientes"] = clientes;
            return View(presupuestoVM);
        }
        [HttpPost]
        public IActionResult EditarPresupuestoPost(ViewEditarPresupuesto presupuestoVM)
        {
            Console.WriteLine($"ClienteId: {presupuestoVM.ClienteId}, Fecha: {presupuestoVM.Fecha}");
            if (!ModelState.IsValid)
                return RedirectToAction("ListarPresupuesto");
            var presupuesto = new Presupuesto(presupuestoVM);
            _presupuestoRepositorio.Update(presupuesto);
            return RedirectToAction("ListarPresupuesto");
        }

   
        //LISTAR PRESUPUESTO--------------------------------------------------

        [HttpGet]
        public IActionResult ListarPresupuesto()
        {
            // Obtiene todos los productos del repositorio
            var presupuesto = _presupuestoRepositorio.getAll();
            return View(presupuesto); // Pasa la lista de productos a la vista
        }

        //ELIMINAR PRESUPUESTO--------------------------------------------------

        [HttpGet]
        public IActionResult EliminarPresupuesto(int idPresupuesto)
        {  
            var presupuesto = _presupuestoRepositorio.GetById(idPresupuesto);
            return View(presupuesto);
        }
        [HttpPost]
         public IActionResult DeletePresupuesto(int idPresupuesto)
        {  
            
            _presupuestoRepositorio.Delete(idPresupuesto);
            return RedirectToAction("ListarPresupuesto"); // Redirige a ListarPresupuesto luego de eliminar
        }
    //CARGAR PRODUCTOS A PRESUPUESTO--------------------------------------------------
   // Cargar productos para agregar al presupuesto
    [HttpGet]
    public IActionResult AgregarProducto(int idPresupuesto){//FIJATE QUE EL NOMBRE QUE LE PASAS DE LISTARPRODUCTO COINICIDA CON ESTE
        var model = new viewAgregarProductoAlPresupuesto();
        model.productos = _productoRepositorio.getAll();
        model.IdPresupuesto = idPresupuesto ;
        return View(model);
    }

    // Acción para asignar producto al presupuesto
    [HttpPost]
    public IActionResult AgregarProductoPost(viewAgregarProductoAlPresupuesto model)
    {
        if (ModelState.IsValid)
        {
            // Aquí se debería ver que el IdPresupuesto está correctamente pasado
            _presupuestoRepositorio.agregarDetalle(model.IdPresupuesto, model.IdProducto, model.cantidad);
        }
        return RedirectToAction("ListarPresupuesto");
    }
    }
}