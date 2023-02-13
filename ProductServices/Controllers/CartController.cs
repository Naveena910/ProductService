using Contracts.IServicees;
using Entities.Dtos.RequestDto;
using Entities.Dtos.ResponseDto;
using Entities.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using System.Net;

namespace ProductServices.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly IProductService _Productservice;
        private readonly ILogger<CartController> _logger;
        public CartController(IProductService Service, ILogger<CartController> logger)
        {
            _Productservice = Service;
            _logger = logger;
        }

        /// <summary>
        /// Add items to cart
        /// </summary>
        /// <param name="cart"></param>
        [HttpPost]
        [Authorize]
        
        public IActionResult AddtoCart([FromBody]CartForCreatingDto cart)
        {
            _logger.LogInformation("Adding products in cart in the database");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid data");
                ErrorDto badRequest = _Productservice.ModelState(ModelState);
                return BadRequest(badRequest);
            }
            try
            {
                ResponseDto Id = _Productservice.AddtoCart(cart);
                _logger.LogInformation("Added to cart successfully");
                return Created("Product created", Id);

            }
            catch (NotFoundException e)
            {
                _logger.LogDebug("No product with this product Id found in the database");
                return NotFound(new ErrorDto { ErrorMessage = "Notfound", StatusCode = (int)HttpStatusCode.NotFound, Description = e.Message });
            }
            catch (Exception)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }


        }
        /// <summary>
        /// Getting Cart items by userId
        /// </summary>
        /// <param name="userId"></param>
       
        [HttpGet("{userId}")]
        public IActionResult GetCartByUserId(Guid userId)
        {
            _logger.LogInformation("Getting items from cart...");

            try
            {
                List<CartDto> userCart = _Productservice.GetCartByUserId(userId);
                if (userCart.Count() == 0)
                {
                    _logger.LogDebug("No items found in cart");
                    return StatusCode(StatusCodes.Status204NoContent, new ErrorDto { ErrorMessage = "No Content", StatusCode = (int)HttpStatusCode.NoContent, Description = "No address found with this user Id" });
                }
                _logger.LogInformation("items in the cart");
                return StatusCode(StatusCodes.Status200OK, userCart);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }

        }
        [HttpPut]
        [Route("{productId}")]
        public IActionResult UpdateCartById([FromRoute] Guid productId, CartForUpdateDto quantity)
        {
            try
            {
                _Productservice.UpdateCartById(productId, quantity);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (NotFoundException e)
            {
                _logger.LogDebug("No product with this product Id found in the database");
                return NotFound(new ErrorDto { ErrorMessage = "Notfound", StatusCode = (int)HttpStatusCode.NotFound, Description = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }
        }
        /// <summary>
        /// Deletes an cart by userId
        /// </summary>
        /// <param name="productId"></param>

        [HttpDelete]
        [Route("{productId}")]
        public IActionResult DeleteByproductId([FromRoute] Guid productId)
        {
            try
            {
                _Productservice.DeleteCartByProductId(productId);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (NotFoundException e)
            {
                _logger.LogDebug("No product with this product Id found in the database");
                return NotFound(new ErrorDto { ErrorMessage = "Notfound", StatusCode = (int)HttpStatusCode.NotFound, Description = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }

        }
        [HttpDelete]
        [AllowAnonymous]
        [Route("delete/{userId}")]
        public IActionResult DeleteByUserId([FromRoute] Guid userId)
        {
         
                _Productservice.DeleteCartByUserId(userId);

                return StatusCode(StatusCodes.Status200OK);
            
        }
    }
}
