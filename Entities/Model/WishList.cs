using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class WishList : CommonModel
    {
        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

    }
}
