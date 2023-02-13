using Contracts.IRepository;
using Contracts.IServicees;
using Entities.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository
        : GenericRepository<Product>, IProductRepository
    {
        protected readonly RepositoryContext _context;
        public ProductRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public bool ProductExists(string productName)
        {
           return  _context.Product.Any(a => (a.Name == productName) && a.IsActive == true);
        }
        /// <summary>
        /// sorting
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<Product> GetProducts(Pagination pagination)
        {
           var sort = _context.Product as IQueryable<Product>;
       if (sort.Any(s=>s.Category==pagination.SortBy.ToLower())&& pagination.SortOrder.ToLower() == "asc")
                return sort.OrderBy(x => x.Category).ToList();
  
            return null;

        }
    }
}
