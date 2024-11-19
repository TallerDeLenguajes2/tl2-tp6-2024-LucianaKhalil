using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_LucianaKhalil.Models;

namespace tl2_tp6_2024_LucianaKhalil.Controllers
{
    public class PresupuestoController : Controller
    {
        private readonly PresupuestoRepositorio _presupuestoRepositorio;
          private readonly ProductoRepositorio _productoRepositorio; // Aquí agregamos el repositorio de productos

    // Constructor
    public PresupuestoController()
    {
        _presupuestoRepositorio = new PresupuestoRepositorio(@"Data Source=db\Tienda.db;Cache=Shared");
        _productoRepositorio = new ProductoRepositorio(@"Data Source=db\Tienda.db;Cache=Shared"); // Iniciamos el repositorio de productos
    }
    
        //CREAR PRESUPUESTO-------------------
        [HttpGet]
        public IActionResult CrearPresupuesto()
        {   
            return View(new Presupuesto());
        }

        [HttpPost]
        public IActionResult CrearPresupuesto(Presupuesto presupuesto)
        {   
            _presupuestoRepositorio.Create(presupuesto);
            return RedirectToAction("ListarPresupuesto"); // Redirige a ListarPresupuesto
        }
        //EDITAR PRESUPUESTO-----------------------------------
        [HttpGet]
        public IActionResult EditarPresupuesto(int idPresupuesto)
        {  
            var presupuesto = _presupuestoRepositorio.GetById(idPresupuesto);
            return View(presupuesto);
        }

        [HttpPost]
        public IActionResult EditarPresupuesto(Presupuesto presupuesto)
        {   
            _presupuestoRepositorio.Update(presupuesto);
            return RedirectToAction("ListarPresupuesto");
        }
   
        //LISTAR PRESUPUESTO----------------------------------
        [HttpGet]
        public IActionResult ListarPresupuesto()
        {
            // Obtiene todos los productos del repositorio
            var presupuesto = _presupuestoRepositorio.getAll();
            return View(presupuesto); // Pasa la lista de productos a la vista
        }
        //ELIMINAR PRESUPUESTO
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
    //CARGAR PRODUCTOS A PRESUPUESTO
   // Cargar productos para agregar al presupuesto
    [HttpGet]
    public IActionResult AgregarProducto(int idPresupuesto)
    {
        var presupuesto = _presupuestoRepositorio.GetById(idPresupuesto); 

        var productos = _productoRepositorio.getAll(); 

        ViewData["Productos"] = productos;
        
        return View(presupuesto);
    }

    // Acción para asignar producto al presupuesto
    [HttpPost]
    public IActionResult AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        _presupuestoRepositorio.agregarDetalle(idPresupuesto, idProducto, cantidad);

        return RedirectToAction("ListarPresupuesto");
    }

 }
}