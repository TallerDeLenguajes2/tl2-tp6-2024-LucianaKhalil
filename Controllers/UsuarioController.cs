using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_LucianaKhalil.Models;

namespace tl2_tp6_2024_LucianaKhalil.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        // Necesito obtener de repositorio usuarios
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult CrearUsuario()
        {   
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult CrearUsuario(Usuario usuario)
        {   
            _usuarioRepository.Create(usuario);
            return RedirectToAction("ListarUsuarios"); // Redirige a ListarUsuarios
        }

        [HttpGet]
        public IActionResult EditarUsuario(int id)
        {  
            var usuario = _usuarioRepository.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuario)
        {   
            _usuarioRepository.Update(usuario);
            return RedirectToAction("ListarUsuarios");
        }

        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            // Obtiene todos los usuarios del repositorio
            var usuarios = _usuarioRepository.getAll();
            return View(usuarios); // Pasa la lista de usuarios a la vista
        }

        [HttpGet]
        public IActionResult DeleteUsuario(int id)
        {  
            var usuario = _usuarioRepository.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {  
            // Elimina el usuario usando el repositorio
            _usuarioRepository.Delete(id);
            return RedirectToAction("ListarUsuarios"); // Redirige a ListarUsuarios
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
