﻿using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IRepository
{
    public interface IWishListRepository : IGenericRepository<WishList>
    {
        
        public WishList GetByProductId(Guid productId);
        public bool checkproduct (Guid productId);



    }
}
