using Contracts.IRepository;
using Entities.Model;
using Microsoft.AspNetCore.Http;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _dbContext;
        public ICartRepository cart { get; }
        public IProductRepository product { get; }
        public IWishListRepository wishlist { get; }
       

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnitOfWork(RepositoryContext dbContext,
                            IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository, IProductRepository productRepository, IWishListRepository wishListRepository)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            cart = cartRepository;
            product = productRepository;
            wishlist = wishListRepository;
            

        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public void CurrentUser(Guid userId)
        {
            Guid currentId = new Guid(_httpContextAccessor.HttpContext.User?.FindFirstValue("userid"));

            if (currentId != userId)
            {
                throw new UnauthorizedException("user not found");
            }
        }
        public void DeleteAll(Guid userId)
        {
            var q=_dbContext.Cart.Where(x=>x.UserId==userId).ToList();
           
            foreach (var item in q)
            {
                _dbContext.Cart.Remove(item);
            }
        }
        public Cart GetCartById(Guid id)
        {
            return _dbContext.Cart.FirstOrDefault(x=>x.ProductId==id);
        }
            public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}
