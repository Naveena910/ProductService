using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos.RequestDto
{
    public class ProductForCreatingDto
    {
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "This field is required")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "This field is required")]
        public string Image { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int Price { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public bool Visibility { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Category { get; set; } = string.Empty;
    }
}
