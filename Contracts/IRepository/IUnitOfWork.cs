using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICartRepository cart { get; }
        IProductRepository product { get; }
        IWishListRepository wishlist{ get; }
      
        void Save();
        public void CurrentUser(Guid userId);
        public void DeleteAll(Guid userId);
        public Cart GetCartById(Guid id);
    }
}
