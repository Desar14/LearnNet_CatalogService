using FluentValidation;
using LearnNet_CatalogService.Api.Auth;
using LearnNet_CatalogService.Api.Models.Category;
using LearnNet_CatalogService.Api.Models.HATEOAS;
using LearnNet_CatalogService.Api.Models.Product;
using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearnNet_CatalogService.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        private readonly LinkGenerator _linkGenerator;

        public ProductsController(ICategoryService categoryService,
                                  IProductService productService,
                                  ILogger<ProductsController> logger,
                                  LinkGenerator linkGenerator)
        {
            _categoryService = categoryService;
            _productService = productService;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }

        // GET: api/<ProductsController>
        [Authorize(Policy = Policies.Read)]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LinkCollectionWrapper<ProductModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int? categoryId, int page = 0, int limit = 50)
        {
            var dTOs = await _productService.GetAllProductsByCategoryIdAsync(categoryId, page, limit);

            var models = dTOs.Select(ProductModel.MapFrom).ToList();

            foreach (var product in models)
            {
                var links = CreateLinksForProduct(product.Id);
                product.Links = links.ToList();
            }

            var productsWrapper = new LinkCollectionWrapper<ProductModel>(models);
            return Ok(CreateLinksForGetAllProducts(productsWrapper));
        }

        // GET api/<ProductsController>/5
        [Authorize(Policy = Policies.Read)]
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            
            if (productDto == null )
                return NotFound();

            var model = ProductModel.MapFrom(productDto);
            model.Links = CreateLinksForProduct(model.Id).ToList();

            return Ok(model);
        }

        // POST api/<ProductsController>
        [Authorize(Policy = Policies.Create)]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProductWriteModel model)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(model.CategoryId);

            if (categoryDto == null)
                return BadRequest();

            var dto = ProductWriteModel.MapTo(model);

            ProductDTO addedDto = await _productService.AddProductAsync(dto);

            var addedModel = ProductModel.MapFrom(addedDto);
            addedModel.Links = CreateLinksForProduct(addedModel.Id).ToList();

            return Created(nameof(GetById), addedModel);
        }

        // PUT api/<ProductsController>/5
        [Authorize(Policy = Policies.Update)]
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductWriteModel model)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(model.CategoryId);

            var productDto = await _productService.GetProductByIdAsync(id);

            if (categoryDto == null || productDto == null)
                return NotFound();

            var dto = ProductWriteModel.MapTo(model);

            dto.Id = id;

            await _productService.UpdateProductAsync(dto);
            
            return await GetById(id);
        }

        // DELETE api/<ProductsController>/5
        [Authorize(Policy = Policies.Delete)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);

            return NoContent();
        }

        private IEnumerable<Link> CreateLinksForProduct(int id)
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetById), values: new { id }) ?? "",
                "self",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Delete), values: new { id })?? "",
                "delete_product",
                "DELETE"),

                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Update), values: new { id }) ?? "",
                "update_product",
                "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<ProductModel> CreateLinksForGetAllProducts(LinkCollectionWrapper<ProductModel> productWrapper)
        {
            productWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Get), values: new { }) ?? "",
                    "self",
                    "GET"));

            return productWrapper;
        }
    }
}
