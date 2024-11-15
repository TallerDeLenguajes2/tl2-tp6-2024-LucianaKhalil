using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_LucianaKhalil.Models;

namespace tl2_tp6_2024_LucianaKhalil.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoRepositorio _productoRepositorio;

        //necesito obtener de repositorio productos
        public ProductosController()
        {
            _productoRepositorio = new ProductoRepositorio(@"Data Source=db\Tienda.db;Cache=Shared");
        }

        [HttpGet]
        public IActionResult CrearProducto()
        {   
            return View(new Productos());
        }

        [HttpPost]
        public IActionResult CrearProducto(Productos producto)
        {   
            _productoRepositorio.Create(producto);
            return RedirectToAction("ListarProductos"); // Redirige a ListarProductos 
        }

        [HttpGet]
        public IActionResult EditarProducto(int idProducto)
        {  
            var producto = _productoRepositorio.GetById(idProducto);
            return View(producto);
        }

        [HttpPost]
        public IActionResult EditarProducto(Productos producto)
        {   
            _productoRepositorio.Update(producto, producto.IdProducto);
            return RedirectToAction("ListarProductos");
        }


        [HttpGet]
        public IActionResult ListarProductos()
        {
            // Obtiene todos los productos del repositorio
            var productos = _productoRepositorio.getAll();
            return View(productos); // Pasa la lista de productos a la vista
        }
    
        public IActionResult DeleteProducto(int idProducto)
        {  
            // Elimina el producto usando el repositorio
            _productoRepositorio.Delete(idProducto);
            return RedirectToAction("ListarProductos"); // Redirige a ListarProductos 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}