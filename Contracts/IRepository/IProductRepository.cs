using Contracts.IServicees;
using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        bool ProductExists(string productName);
        /// <summary>
        /// sorting
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<Product> GetProducts(Pagination pagination);
    }
}
