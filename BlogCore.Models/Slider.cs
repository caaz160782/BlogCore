using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [Display(Name = "Nombre del Slider")]
        [StringLength(100, ErrorMessage = "El campo Nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El campo estado es obligatorio.")]
        [Display(Name = "Va estar activo el Slider")]
        public bool estado { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }
    }
}
