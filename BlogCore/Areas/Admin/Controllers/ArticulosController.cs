using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
        
            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM artiVm)
        {
            ModelState.Remove("Articulo.UrlImagen");
            ModelState.Remove("Articulo.Categoria");
            ModelState.Remove("ListaCategorias");
            if (ModelState.IsValid)
            {
                string rutaPRincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if(artiVm.Articulo.Id == 0 && archivos.Count() > 0 )
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPRincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVm.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    artiVm.Articulo.FechaCreacion = DateTime.Now;
                    _contenedorTrabajo.Articulo.Add(artiVm.Articulo);
                    _contenedorTrabajo.Save();
                    return RedirectToAction("Index");
                }else
                {
                    ModelState.AddModelError("Imagen", "se debe seleccionar una imagen");
                }
            }
            artiVm.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVm);
        }


        [HttpGet]
        public IActionResult Edit(int? id) {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            if (id != null)
            {
                articuloVM.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
                return View(articuloVM);
            }
            else
            {
                return NotFound();
            }                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM artiVm)
        {
            ModelState.Remove("Articulo.UrlImagen");
            ModelState.Remove("Articulo.Categoria");
            ModelState.Remove("ListaCategorias");
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(artiVm.Articulo.Id);

                if (archivos.Count() > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);
                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));
                    
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVm.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    artiVm.Articulo.FechaCreacion = DateTime.Now;
                  
                }
                else
                {
                   artiVm.Articulo.UrlImagen = articuloDesdeDb.UrlImagen;
                  
                }
                _contenedorTrabajo.Articulo.Update(artiVm.Articulo);
                _contenedorTrabajo.Save();
                return RedirectToAction("Index");
            }
            artiVm.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVm);
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria");
            return Json(new { data = allObj });
        }

       [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            
            if (articuloDesdeDb == null)
            {
                return Json(new { success = false, message = "Error borrando archivo" });
            }
            _contenedorTrabajo.Articulo.Remove(articuloDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Se ha borrado el archivo" });
        }
        #endregion
    }
}
