using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class UsuarioController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        
        public UsuarioController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
         
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            // Aquí puedes obtener la lista de usuarios desde la base de datos
            //return View(_contenedorTrabajo.Usuario.GetAll());
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var id = claim.Value;
            var usuario = _contenedorTrabajo.Usuario.GetFirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            var usuarios = _contenedorTrabajo.Usuario.GetAll(u => u.Id != id);
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            _contenedorTrabajo.Save();
            return RedirectToAction("Index");   
        }

        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            _contenedorTrabajo.Save();
            return RedirectToAction("Index");
        }

    }
}
