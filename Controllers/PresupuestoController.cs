using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_LucianaKhalil.Models;

namespace tl2_tp6_2024_LucianaKhalil.Controllers
{
    public class PresupuestoController : Controller
    {
        private readonly PresupuestoRepositorio _presupuestoRepositorio;

        //necesito obtener de repositorio productos
        public PresupuestoController()
        {
            _presupuestoRepositorio = new PresupuestoRepositorio(@"Data Source=db\Tienda.db;Cache=Shared");
        }
        [HttpGet]
        public IActionResult CrearPresupuesto()
        {   
            return View(new Presupuesto());
        }

        [HttpPost]
        public IActionResult CrearProducto(Presupuesto presupuesto)
        {   
            _presupuestoRepositorio.Create(presupuesto);
            return RedirectToAction("ListarPresupuesto"); // Redirige a ListarPresupuesto
        }
        //editar presupuesto-----------------------------------
        
        

    }
}