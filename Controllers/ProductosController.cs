using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_LucianaKhalil.Models;

namespace tl2_tp6_2024_LucianaKhalil.Controllers;

public class ProductosController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static List<Productos> productos = new List<Productos>();
     
    public ProductosController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var indexProductoViewModel = new IndexProductoViewModel(productos);
        return View(indexProductoViewModel);
    }

    [HttpGet]
    public IActionResult CrearProducto()
    {   
        return View(new Productos());
    }


    [HttpPost]
    public IActionResult CrearProducto(Productos producto)
    {   
        producto.IdProducto = productos.Count()+1;
        productos.Add(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarProducto(int idProducto)
    {  
        return View( productos.FirstOrDefault( producto => producto.IdProducto ==idProducto));
    }

     [HttpGet]
    public IActionResult ListarProductos()
    {
        return View(productos); // Pasas la lista de productos a la vista
    }

    [HttpPost]
    public IActionResult EditarProducto(Productos producto)
    {   
        
        var producto2 = productos.FirstOrDefault( producto => producto.IdProducto == producto.IdProducto);
        producto2.Descripcion=producto.Descripcion;
        producto2.Precio = producto.Precio;

        return RedirectToAction("Index");
    }

    
    public IActionResult DeleteProducto(int idProducto)
    {  
       var productoBuscado = productos.FirstOrDefault( producto => producto.IdProducto == idProducto);
       productos.Remove(productoBuscado);
      return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}