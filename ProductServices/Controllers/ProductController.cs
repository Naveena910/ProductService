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
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _Productservice ;
        private readonly ILogger<ProductController> _logger;
      public ProductController(IProductService Service, ILogger<ProductController> logger)
        {
            _Productservice= Service;
            _logger = logger;
        }

        /// <summary>
        /// Creates a product by admin
        /// </summary>
        /// <param name="product"></param>
        [HttpPost]
        [Authorize]
        
        public IActionResult CreateProduct([FromBody] ProductForCreatingDto product)
        {
            _logger.LogInformation("Creating product in the database");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid data");
                ErrorDto badRequest = _Productservice.ModelState(ModelState);
                return BadRequest(badRequest);
            }
            try
            {
                ResponseDto Id =_Productservice.CreateProduct(product);
                _logger.LogInformation("Your product created successfully");
                return Created("Product created", Id);

            }
            catch (ConflictException e)
            {
                _logger.LogDebug("Product with this name a already exists in the database");
                return Conflict(new ErrorDto { ErrorMessage = "Conflict", StatusCode = (int)HttpStatusCode.Conflict, Description = e.Message });
            }
            catch (ForbiddenException)

            {
                _logger.LogDebug("Access Denied");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDto { ErrorMessage = "Forbidden", StatusCode = (int)HttpStatusCode.Forbidden, Description = "Access denied" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }


        }
        /// <summary>
        /// Get the product by product id
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        
        [Route("{id:guid}")]
        public IActionResult GetProductById([FromRoute] Guid id)
        {
            try
            {
                ProductDto product = _Productservice.GetProductById(id);
                _logger.LogInformation("Getting product by ID");
                return StatusCode(StatusCodes.Status200OK, product);
            }
            catch (NotFoundException e)
            {
                _logger.LogDebug("No product with this product Id found in the database");
                return NotFound(new ErrorDto { ErrorMessage = "Notfound", StatusCode = (int)HttpStatusCode.NotFound, Description = e.Message });
            }
            catch (Exception)
            {
                _logger.LogError("Something went  wrong");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }

        }
        /// <summary>
        /// Getting all products by category
        /// </summary>
       
        [HttpGet]
        public IActionResult GetAllProducts([FromQuery]Pagination pagination)
        {
            try
            {
                PagedList<ProductDto> products =_Productservice.GetAll(pagination);

                if (products.Count == 0)
                {
                    _logger.LogDebug("No products found");
                    return StatusCode(StatusCodes.Status204NoContent, new ErrorDto { ErrorMessage = "No Content", StatusCode = (int)HttpStatusCode.NoContent, Description = "No address found with this user Id" });
                }
                _logger.LogInformation("Getting all products ");
                return StatusCode(StatusCodes.Status200OK, products);
            }
            catch (Exception)
            {
                _logger.LogError("Something went  wrong");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }

        }
        /// <summary>
        /// Update  product details by product id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateUser([FromRoute] Guid id, [FromBody] ProductForUpdateDto product)
        {
            try
            {
                _Productservice.UpdateProduct(id, product);
                _logger.LogInformation("Updated product successfully");
                return StatusCode(StatusCodes.Status200OK);
            }

            catch (NotFoundException e)
            {
                _logger.LogDebug("No product with this product Id found in the database");
                return NotFound(new ErrorDto { ErrorMessage = "Notfound", StatusCode = (int)HttpStatusCode.NotFound, Description = e.Message });
            }
            catch (ForbiddenException)

            {
                _logger.LogDebug("Access Denied");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDto { ErrorMessage = "Forbidden", StatusCode = (int)HttpStatusCode.Forbidden, Description = "Access denied" });
            }
            catch (Exception)
            {
                _logger.LogError("Something went  wrong");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }
        }
        /// <summary>
        /// Deletes an product by product id
        /// </summary>
        /// <param name="productId"></param>
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteByProductId([FromRoute] Guid id)
        {
            try
            {
                _Productservice.DeleteByProductId(id);
                _logger.LogInformation("Deleted a product with this product id");
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (NotFoundException e)
            {
                _logger.LogDebug("No product with this product Id found in the database");
                return NotFound(new ErrorDto { ErrorMessage = "Notfound", StatusCode = (int)HttpStatusCode.NotFound, Description = e.Message });
            }
            catch (ForbiddenException)

            {
                _logger.LogDebug("Access Denied");
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorDto { ErrorMessage = "Forbidden", StatusCode = (int)HttpStatusCode.Forbidden, Description = "Access denied" });
            }
            catch (Exception)
            {
                _logger.LogError("Something went  wrong");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDto { ErrorMessage = "IntenalServerError", StatusCode = (int)HttpStatusCode.InternalServerError, Description = "Something went wrong" });
            }

        }

    }
}
