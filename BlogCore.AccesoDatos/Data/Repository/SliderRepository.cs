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
    internal class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;
        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
           

        public void Update(Slider slider)
        {
            var objFromDb = _db.Slider.FirstOrDefault(u => u.Id == slider.Id);
            objFromDb.Nombre      = slider.Nombre;
            objFromDb.estado = slider.estado;
            objFromDb.UrlImagen   = slider.UrlImagen;
           

        }
    }   
    
}
