using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IRepository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        public void DeleteCart(Guid userId);

    }
}
