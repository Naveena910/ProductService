using Contracts.IServicees;
using Entities.Dtos.RequestDto;
using Entities.Dtos.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using System.Net;

namespace ProductServices.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/wishlist")]
    public class WishListController : ControllerBase
    {
        private readonly IProductService _Productservice;
        private readonly ILogger<WishListController> _logger;
        public WishListController(IProductService Service, ILogger<WishListController> logger)
        {
            _Productservice = Service;
            _logger = logger;
        }
        /// <summary>
        /// Add items to wishlist
        /// </summary>
        /// <param name="wishlist"></param>
        [HttpPost]
        [Authorize]
       
        public IActionResult AddtoWishlist([FromBody] WishListCreatingDto wishlist)
        {
            _logger.LogInformation("Creating wishlist in the database");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid data");
                ErrorDto badRequest = _Productservice.ModelState(ModelState);
                return BadRequest(badRequest);
            }
            try
            {
                ResponseDto Id = _Productservice.AddtoWishlist(wishlist);
                _logger.LogInformation("Your wishlist created successfully");
                return Created("Added to wishlist", Id);
            }
            catch (ConflictException e)
            {
                _logger.LogDebug("Product with this name a already exists in the database");
                return Conflict(new ErrorDto { ErrorMessage = "Conflict", StatusCode = (int)HttpStatusCode.Conflict, Description = e.Message });
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
        /// Getting wishlist items by userId
        /// </summary>
       
        [HttpGet]
        public IActionResult GetCartByUserId()
        {
            _logger.LogInformation("Getting items from wishlist...");

            try
            {
                List<WishListDto> userwishlist = _Productservice.GetWishListByUserId();
                if (userwishlist.Count() == 0)
                {
                    _logger.LogDebug("No wishlist found with this user Id.");
                    return StatusCode(StatusCodes.Status204NoContent, new ErrorDto { ErrorMessage = "No Content", StatusCode = (int)HttpStatusCode.NoContent, Description = "No address found with this user Id" });
                }
                _logger.LogInformation("Getting addresss...Completed");
                return StatusCode(StatusCodes.Status200OK, userwishlist);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }

        }
        /// <summary>
        /// Deletes an wishlist by userId
        /// </summary>
        /// <param name="productId"></param>

        [HttpDelete]
        [Route("{productId}")]
        public IActionResult DeleteByProductId([FromRoute]Guid productId)
        {
            try
            {
                _Productservice.DeleteBywishlistproductId(productId);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (NotFoundException e)
            {
                _logger.LogDebug("No product found with this product Id in the database");
                return NotFound(new ErrorDto { ErrorMessage = "Notfound", StatusCode = (int)HttpStatusCode.NotFound, Description = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }

        }
    }
}