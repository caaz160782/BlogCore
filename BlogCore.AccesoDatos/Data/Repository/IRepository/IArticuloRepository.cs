using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    // Ensure there is no duplicate definition of IArticuloRepository in this namespace.
    public interface IArticuloRepository : IRepository<Articulo>
    {
         void Update(Articulo articulo);
    }
    
    
}
