using Microsoft.AspNetCore.Mvc;
namespace tl2_tp6_2024_LucianaKhalil.Controllers{
public class ClienteController : Controller
{
    private readonly IClienteRepository _clienteRepositorio;

    public ClienteController()
    {
        _clienteRepositorio = new ClienteRepositorio(@"Data Source=db\Tienda.db;Cache=Shared");
    }

    [HttpGet]
    public IActionResult ListarCliente()
    {
        var clientes = _clienteRepositorio.getAll();
        return View(clientes);
    }

    [HttpGet]
    public IActionResult CrearCliente()
    {
        return View(new ViewCliente());
    }

    [HttpPost]
    public IActionResult CrearCliente(ViewCliente cliente)
    {
        if (ModelState.IsValid)
        {
            var c = new Cliente(cliente);
            _clienteRepositorio.Create(c);
            return RedirectToAction("ListarCliente");
        }

        return View(new ViewCliente());
    }

    [HttpGet]
    public IActionResult EditarCliente(int id)
    {
        var cliente = _clienteRepositorio.GetById(id);
        ViewCliente model = new ViewCliente(cliente);
        return View(model);
    }

    [HttpPost]
    public IActionResult EditarClientePost(int id, ViewCliente clienteVM)
    {
        if (ModelState.IsValid)
        {   
            var c = new Cliente(clienteVM);
            _clienteRepositorio.Update(c);
            return RedirectToAction("ListarCliente");
        }

        return View(new ViewCliente());
    }

    [HttpGet]//aca no uso vm
    public IActionResult DeleteCliente(int id)
    {
        var eliminado = _clienteRepositorio.GetById(id);
        if (eliminado == null)
        {
            return RedirectToAction("ListarCliente", new { error = "Cliente no encontrado" });
        }
        return View(eliminado);
    }
    [HttpPost]
    public IActionResult EliminarClientePost(int id)
    {
        _clienteRepositorio.Delete(id);
        return RedirectToAction("ListarCliente");
    }
}
}

