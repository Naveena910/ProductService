using Entities.Model;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }
        public DbSet<Product> Product { get; set; }



        public DbSet<Cart> Cart { get; set; }

        public DbSet<WishList> WishList { get; set; }
       

    }
}