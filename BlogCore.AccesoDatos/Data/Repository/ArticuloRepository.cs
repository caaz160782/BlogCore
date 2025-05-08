using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    internal class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        private readonly ApplicationDbContext _db;
        public ArticuloRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
           

        public void Update(Articulo articulo)
        {
            var objFromDb = _db.Articulo.FirstOrDefault(u => u.Id == articulo.Id);
            objFromDb.Nombre = articulo.Nombre;
            objFromDb.Descripcion = articulo.Descripcion;
            objFromDb.UrlImagen = articulo.UrlImagen;
            objFromDb.CategoriaId = articulo.CategoriaId;

        }
    }   
    
}
