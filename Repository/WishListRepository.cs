using Contracts.IRepository;
using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WishListRepository
       : GenericRepository<WishList>, IWishListRepository
    {
        protected readonly RepositoryContext _context;
        public WishListRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
      
        public WishList GetByProductId(Guid productId)
        {
            return _context.WishList.FirstOrDefault(x=>x.ProductId==productId);
        }
        public bool checkproduct(Guid productId)
        {
            return _context.WishList.Any(x=>x.ProductId==productId);
        }
        public void DeleteCart(Guid userId)
        {

            var cart = _context.Cart.FirstOrDefault(s => s.ProductId == userId);
            if (cart != null)
            {
                _context.Remove(cart);
                _context.SaveChanges();
            }
        }

    }
}
