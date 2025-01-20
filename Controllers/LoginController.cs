using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_LucianaKhalil.ViewModels;
using System.Linq;

namespace tl2_tp6_2024_LucianaKhalil.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
             return View("Login", new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = _usuarioRepository
                    .getAll()
                    .FirstOrDefault(u => u.NombreUsuario == model.NombreUsuario && u.Password == model.Password);

                if (usuario != null)
                {
                    if (usuario.RolUsuario == Rol.Admin)
                    {
                        return RedirectToAction("EditarPresupuesto", "Presupuesto");
                    }
                    else if (usuario.RolUsuario == Rol.Cliente)
                    {
                        return RedirectToAction("ListarPresupuesto", "Presupuesto");
                    }
        }

                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            }
            return View(model);

        }
    }
}
