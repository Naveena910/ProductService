using Contracts.IRepository;
using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CartRepository
        : GenericRepository<Cart>, ICartRepository
    {
        protected readonly RepositoryContext _context;
        public CartRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public void DeleteCart(Guid  userId)
        {

            var cart= _context.Cart. FirstOrDefault(s => s.ProductId == userId);
            if (cart != null)
            {
               _context.Remove(cart);
               _context.SaveChanges();
            }
        }
    }
}
