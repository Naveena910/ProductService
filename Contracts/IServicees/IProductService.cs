using Entities.Dtos.RequestDto;
using Entities.Dtos.ResponseDto;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IServicees
{
    public interface IProductService
    {
        ///<summary>
        /// checks validation with the user given data.
        ///</summary>
        ///<return>ErrorDto</return>
        public ErrorDto ModelState(ModelStateDictionary modelState);


        /// <summary>
        ///checks user or admin
        /// </summary>
        /// <param name="userId"></param>
        public Guid UserTypecheck();
        /// <summary>
        ///creates products in the database
        /// </summary>
        /// <param name="product"></param>
        public ResponseDto CreateProduct( ProductForCreatingDto product);
        /// <summary>
        /// Gets the product deatils by Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductDto GetProductById(Guid userId);
        /// <summary>
        /// for getting all the users from the database
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        //get all the users
        public PagedList<ProductDto> GetAll(Pagination pagination);
        /// <summary>
        /// Update by product id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userUpdate"></param>
        public void UpdateProduct(Guid productId, ProductForUpdateDto userUpdate);
        /// <summary>
        /// Delete  by Product id
        /// </summary>
        /// <param name="productId"></param>

        public void DeleteByProductId(Guid productId);
        /// <summary>
        /// Adds a product to cart
        /// </summary>
        /// <param name="addtoCart"></param>
        public ResponseDto AddtoCart(CartForCreatingDto addtoCart);
        /// <summary>
        /// Gets all  cart items by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CartDto> GetCartByUserId(Guid userId);
        /// <summary>
        /// update a product to cart
        /// </summary>
        /// <param name="quantity"></param>
        public void UpdateCartById(Guid productId, CartForUpdateDto quantity);
        /// <summary>
        /// Delete  by user id
        /// </summary>
        /// <param name="userId"></param>

        public void DeleteCartByProductId(Guid userId);
          public void DeleteCartByUserId(Guid userId);
        public ResponseDto AddtoWishlist(WishListCreatingDto wishListCreatingDto);
        /// <summary>
        /// Gets all  wishlist items by user id
        /// </summary>
        /// <param name="wishlistId"></param>
        /// <returns></returns>
        public List<WishListDto> GetWishListByUserId();
        /// <summary>
        /// Delete  by product id
        /// </summary>
        /// <param name="wishlistproductId"></param>

        public void DeleteBywishlistproductId(Guid wishlistproductId);




        }
    }
