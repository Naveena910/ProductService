using AutoMapper;
using Contracts.IRepository;
using Contracts.IServicees;
//using Contracts.IServices;
using Entities.Dtos.RequestDto;
using Entities.Dtos.ResponseDto;
using Entities.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Client;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IWishListRepository _wishListRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserClient _userClient;
        public ProductService(IMapper mapper, ILogger<ProductService> logger, IUnitOfWork unitOfWork, IProductRepository productRepository, ICartRepository cartRepository, IWishListRepository wishListRepository, IHttpContextAccessor httpContextAccessor,UserClient userClient)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _wishListRepository = wishListRepository;
            _httpContextAccessor = httpContextAccessor;
            _userClient= userClient;
        }
        public ErrorDto ModelState(ModelStateDictionary ModelState)
        {
            return new ErrorDto
            {
                ErrorMessage = ModelState.Keys.FirstOrDefault(),
                Description = ModelState.Values.Select(src => src.Errors.Select(src => src.ErrorMessage).FirstOrDefault()).FirstOrDefault(),
                StatusCode = 400

            };
        }
        /// <summary>
        ///checks user or admin
        /// </summary>
        public Guid UserTypecheck()
        {
            string s = (_httpContextAccessor.HttpContext.User?.FindFirstValue("UserType"));
            if (s == "Customer") throw new ForbiddenException("Access denied");
            return new Guid(_httpContextAccessor.HttpContext.User?.FindFirstValue("userid"));
        }
        /// <summary>
        /// Create an Product in the database 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public ResponseDto CreateProduct(ProductForCreatingDto product)
        {
            _logger.LogInformation("Creating account in service");
            Guid id = UserTypecheck();

            if (_productRepository.ProductExists(product.Name))
            {
                _logger.LogDebug("product already found in the database");
                throw new ConflictException("Product already exists");
            }

            Product products = _mapper.Map<Product>(product);
            products.UserId = id;
            _unitOfWork.product.Add(products);
            _unitOfWork.Save();
            _logger.LogInformation("Product details added from service");
            return new ResponseDto { Id = products.Id };


        }
         
        /// <summary>
        /// Gets the product deatils by Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductDto GetProductById(Guid productId)
        {
            _logger.LogInformation("Getting products in DB");
            Product product = _unitOfWork.product.GetById(productId);

            if (product == null)
            {
                _logger.LogDebug("Product Not found");
                throw new NotFoundException("Product Not Found");
            }
            ProductDto product1 = _mapper.Map<ProductDto>(product);

            return product1;
        }
        /// <summary>
        /// for getting all the products by category from the database
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        //get all the users
        public PagedList<ProductDto> GetAll(Contracts.IServicees.Pagination pagination)
        {

            List<Product> product = _productRepository.GetProducts(pagination);
            if (product == null)
                return PagedList<ProductDto>.Create(new List<ProductDto>(), 0, 1);
            List<ProductDto> products = _mapper.Map<List<ProductDto>>(product);

            return PagedList<ProductDto>.Create(products, pagination.pageNumber, pagination._pageSize);
        }
        /// <summary>
        /// Update by product id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userUpdate"></param>
        public void UpdateProduct(Guid productId, ProductForUpdateDto userUpdate)
        {
           
            Guid id = UserTypecheck();
            Product user = _unitOfWork.product.GetById(productId);
            if (user == null)
            {
                throw new NotFoundException("Product not found");
            }
            user.Category = userUpdate.Category;
            user.DateUpdated = DateTime.Now;
            user.Description = userUpdate.Description;
            user.Name = userUpdate.Name;
            user.Price = userUpdate.Price;
            user.Image = Convert.FromBase64String(userUpdate.Image);
            user.UserId = id;
            _unitOfWork.product.Update(user);
            _unitOfWork.Save();

        }
        /// <summary>
        /// Delete  by Product id
        /// </summary>
        /// <param name="productId"></param>

        public void DeleteByProductId(Guid productId)
        {
            Guid id = UserTypecheck();
            Product product = _unitOfWork.product.GetById(productId);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }
            _unitOfWork.product.Delete(product);
            _unitOfWork.Save();
        }
        /// <summary>
        /// Adds a product to cart
        /// </summary>
        /// <param name="addtoCart"></param>
        public ResponseDto AddtoCart(CartForCreatingDto addtoCart)
        {
            Guid userId = UserTypecheck();
            if (userId == null) throw new NotFoundException("User not found");

            Product product = _unitOfWork.product.GetById(addtoCart.ProductId);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }
            Cart cart = _unitOfWork.GetCartById(addtoCart.ProductId);

            if (cart != null)
            {

                cart.Quantity = addtoCart.Quantity;
                cart.UserId = userId;
                _unitOfWork.cart.Update(cart);
                _unitOfWork.Save();
                return new ResponseDto { Id = cart.Id };
            }
            else
            {
                Cart carts = _mapper.Map<Cart>(addtoCart);
                carts.UserId = userId;
                _unitOfWork.cart.Add(carts);
                _unitOfWork.Save();
                _logger.LogInformation("Cart details added from service");
                return new ResponseDto { Id = carts.Id };
            }
        }
        public ResponseDto AddtoWishlist(WishListCreatingDto wishListCreatingDto)
        {
            Guid userId = UserTypecheck();
            if (userId == null) throw new NotFoundException("User not found");

            Product product = _unitOfWork.product.GetById(wishListCreatingDto.ProductId);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }
            if (_wishListRepository.checkproduct(wishListCreatingDto.ProductId))
            {
                throw new ConflictException("Wishlist with this product already exists");
            }
            WishList wishList1 = _mapper.Map<WishList>(wishListCreatingDto);
            wishList1.UserId = userId;
            _unitOfWork.wishlist.Add(wishList1);
            _unitOfWork.Save();
            _logger.LogInformation("WishList added from service");
            return new ResponseDto { Id = wishList1.Id };

        }
        /// <summary>
        /// Gets all  wishlist items by user id
        /// </summary>
        /// <returns></returns>
        public List<WishListDto> GetWishListByUserId()
        {
            Guid Id = UserTypecheck();
            if (Id == null) throw new NotFoundException("User not found");
            IEnumerable<WishList> wishlistItems = _unitOfWork.wishlist.GetAllById(x => x.UserId ==Id);
            return _mapper.Map<List<WishListDto>>(wishlistItems);

        }
        /// <summary>
        /// Gets all  cart items by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CartDto> GetCartByUserId(Guid userId)
        {
         //  Guid Id = UserTypecheck();
           _userClient.GetUserId(userId);
            IEnumerable<Cart> cartItems = _unitOfWork.cart.GetAllById(x => x.UserId ==userId);
            return _mapper.Map<List<CartDto>>(cartItems);

        }
        public void DeleteCartByUserId(Guid userId)
        {
            _unitOfWork.DeleteAll(userId);
            _unitOfWork.Save();
        }
        /// <summary>
        /// update a product to cart
        /// </summary>
        /// <param name="quantity"></param>
        public void UpdateCartById(Guid productId, CartForUpdateDto quantity)
        {
            Guid userId = UserTypecheck();
            if (userId == null) throw new NotFoundException("User not found");
            Cart cart = _unitOfWork.GetCartById(productId);
            if (cart == null)
            {
                throw new NotFoundException("User not found");
            }
            cart.ProductId = productId;
            cart.Quantity = quantity.Quantity;
            cart.DateUpdated = DateTime.UtcNow;
            _unitOfWork.cart.Update(cart);
            _unitOfWork.Save();

        }
        /// <summary>
        /// Delete  by cartproduct id
        /// </summary>
        /// <param name="cartproductId"></param>

        public void DeleteCartByProductId(Guid cartproductId)
        {
            Guid Id = UserTypecheck();
            if (Id == null) throw new NotFoundException("User not found");
          
            IEnumerable<Cart> usercart = _unitOfWork.cart.GetAllById(x => x.UserId == cartproductId);

            if (usercart == null)
            {
                throw new NotFoundException("No address has been found");
            }
            _unitOfWork.cart.DeleteCart(cartproductId);
            _unitOfWork.Save();
        }

        /// <summary>
        /// Delete  by product id
        /// </summary>
        /// <param name="wishlistproductId"></param>

        public void DeleteBywishlistproductId(Guid wishlistproductId)
        {
            Guid Id = UserTypecheck();
            if (Id == null) throw new NotFoundException("User not found");
            WishList usercart = _wishListRepository.GetByProductId(wishlistproductId);

            if (usercart == null)
            {
                throw new NotFoundException("No address has been found");
            }
            _unitOfWork.wishlist.Delete(usercart);
            _unitOfWork.Save();
        }

    }
}