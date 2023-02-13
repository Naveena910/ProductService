using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos.RequestDto
{
    public class CartForCreatingDto
    {
        [Required(ErrorMessage = "This field is required")]
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int Quantity { get; set; }
    }
}
