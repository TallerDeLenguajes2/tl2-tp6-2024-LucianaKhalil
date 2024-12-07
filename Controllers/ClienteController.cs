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
    public IActionResult ListarClientes()
    {
        var clientes = _clienteRepositorio.getAll;
        return View(clientes);
    }

    [HttpGet]
    public IActionResult CrearCliente()
    {
        return View(new Cliente());
    }

    [HttpPost]
    public IActionResult CrearCliente(Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            _clienteRepositorio.Create(cliente);
            return RedirectToAction("ListarClientes");
        }

        return View(cliente);
    }

    [HttpGet]
    public IActionResult EditarCliente(int id)
    {
        var cliente = _clienteRepositorio.GetById(id);
        return View(cliente);
    }

    [HttpPost]
    public IActionResult EditarCliente(Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            _clienteRepositorio.Update(cliente);
            return RedirectToAction("ListarClientes");
        }

        return View(cliente);
    }

    [HttpPost]
    public IActionResult EliminarCliente(int id)
    {
        _clienteRepositorio.Delete(id);
        return RedirectToAction("ListarClientes");
    }
}
}

