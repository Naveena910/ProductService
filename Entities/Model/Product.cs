using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class Product : CommonModel
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public byte[] Image { get; set; }

        public int Price { get; set; }

        public bool Visibility { get; set; }

        public string Category { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
