using LearnNet_CatalogService.Api.Auth;
using LearnNet_CatalogService.Api.Models.HATEOAS;
using LearnNet_CatalogService.Api.Models.Product;
using LearnNet_CatalogService.Api.Models.ProductDetails;
using LearnNet_CatalogService.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnNet_CatalogService.Api.Controllers
{
    [Route("api/productdetails")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductDetailsController> _logger;
        private readonly LinkGenerator _linkGenerator;

        public ProductDetailsController(IProductService productService,
                                  ILogger<ProductDetailsController> logger,
                                  LinkGenerator linkGenerator)
        {
            _productService = productService;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductDetailsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);

            if (productDto == null)
                return NotFound();

            var model = new ProductDetailsModel
            {
                ProductId = id
            };

            return Ok(model);
        }
    }
}
