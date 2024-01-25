using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/carts/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController( ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]

        public async Task<IActionResult>  GetAllCartItems()
        {
           var allCartItems = await _productService.getCartItems();

            return Ok(allCartItems);
        }

        [HttpGet("id")]

        public async Task<IActionResult> GetCartItemById(int Id)
        {
            try
            {
                var item = await _productService.GetCartItemById(Id);
                return Ok(item);
            } catch {
                return Ok("Unable to fecth resource");
            }
            
        }


       
    }


}
