using FluentValidation;
using LearnNet_CatalogService.Api.Models.Category;
using LearnNet_CatalogService.Api.Models.HATEOAS;
using LearnNet_CatalogService.Api.Models.Product;
using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearnNet_CatalogService.Api.Controllers
{
    [Route("api/category/{categoryId}/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly IValidator<ProductDTO> _validator;

        public ProductsController(ICategoryService categoryService,
                                  IProductService productService,
                                  ILogger<ProductsController> logger,
                                  LinkGenerator linkGenerator,
                                  IValidator<ProductDTO> validator)
        {
            _categoryService = categoryService;
            _productService = productService;
            _logger = logger;
            _linkGenerator = linkGenerator;
            _validator = validator;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LinkCollectionWrapper<ProductModel>) ,StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int categoryId, int page = 0, int limit = 50)
        {
            var dTOs = await _productService.GetAllProductsByCategoryIdAsync(categoryId, page, limit);

            var models = dTOs.Select(ProductModel.MapFrom).ToList();

            foreach (var product in models)
            {
                var links = CreateLinksForProduct(categoryId, product.Id);
                product.Links = links.ToList();
            }


            var productsWrapper = new LinkCollectionWrapper<ProductModel>(models);
            return Ok(CreateLinksForGetAllProducts(productsWrapper));
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int categoryId, int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            var categoryDto = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (productDto == null || categoryDto == null)
                return NotFound();

            var model = ProductModel.MapFrom(productDto);
            model.Links = CreateLinksForProduct(categoryId, model.Id).ToList();

            return Ok(model);
        }

        // POST api/<ProductsController>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(int categoryId, [FromBody] ProductWriteModel model)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (categoryDto == null)
                return NotFound();

            if (model == null)
                return BadRequest();

            var dto = ProductWriteModel.MapTo(model);
            dto.CategoryId = categoryId;

            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            ProductDTO? addedDto = null;

            try
            {
                addedDto = await _productService.AddProductAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Add product error");
                throw;
            }

            if (addedDto == null)
            {
                return StatusCode(500);
            }

            var addedModel = ProductModel.MapFrom(addedDto);
            addedModel.Links = CreateLinksForProduct(categoryId, addedModel.Id).ToList();

            return Created(nameof(GetById), addedModel);
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int categoryId, int id, [FromBody] ProductWriteModel model)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(categoryId);

            var productDto = await _productService.GetProductByIdAsync(id);

            if (categoryDto == null || productDto == null)
                return NotFound();

            var dto = ProductWriteModel.MapTo(model);
            dto.CategoryId = categoryId;

            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            bool addedSuccess = false;

            dto.Id = id;

            try
            {
                addedSuccess = await _productService.UpdateProductAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update category error");
                throw;
            }

            if (!addedSuccess)
            {
                return StatusCode(500);
            }

            return await GetById(categoryId, id);
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int categoryId, int id)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(categoryId);
            var productDto = await _productService.GetProductByIdAsync(id);

            if (categoryDto == null || productDto == null)
                return NotFound();

            var result = await _productService.DeleteProductAsync(id);

            if (!result)
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        private IEnumerable<Link> CreateLinksForProduct(int categoryId, int id)
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetById), values: new { categoryId, id }) ?? "",
                "self",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(CategoryController.GetById), values: new { id = categoryId }) ?? "",
                "related",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Delete), values: new { categoryId, id })?? "",
                "delete_category",
                "DELETE"),

                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Update), values: new { categoryId, id }) ?? "",
                "update_category",
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
