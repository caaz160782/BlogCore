using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
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
            return View(_contenedorTrabajo.Usuario.GetAll());
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
