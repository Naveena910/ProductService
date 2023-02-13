using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos.ResponseDto
{
    public class WishListDto
    {
        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }
    }
}
