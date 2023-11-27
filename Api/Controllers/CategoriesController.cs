using LearnNet_CatalogService.Api.Auth;
using LearnNet_CatalogService.Api.Models.Category;
using LearnNet_CatalogService.Api.Models.HATEOAS;
using LearnNet_CatalogService.Api.Models.Product;
using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearnNet_CatalogService.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILogger<CategoriesController> _logger;
        private readonly LinkGenerator _linkGenerator;

        public CategoriesController(ICategoryService categoryService,
                                  ILogger<CategoriesController> logger,
                                  LinkGenerator linkGenerator,
                                  IProductService productService)
        {
            _categoryService = categoryService;
            _logger = logger;
            _linkGenerator = linkGenerator;
            _productService = productService;
        }



        // GET: api/<CategoryController>
        [Authorize(Policy = Policies.Read)]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LinkCollectionWrapper<CategoryModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var categoriesDto = await _categoryService.GetAllCategoriesAsync();
            var categoryModels = categoriesDto.Select(CategoryModel.MapFrom).ToList();

            foreach (var category in categoryModels)
            {
                var links = CreateLinksForCategory(category.Id);
                category.Links = links.ToList();
            }

            var categoriesWrapper = new LinkCollectionWrapper<CategoryModel>(categoryModels);
            return Ok(CreateLinksForGetAllCategories(categoriesWrapper));

        }

        // GET api/<CategoryController>/5
        [Authorize(Policy = Policies.Read)]
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryDto == null)
                return NotFound();

            var model = CategoryModel.MapFrom(categoryDto);
            model.Links = CreateLinksForCategory(model.Id).ToList();

            return Ok(model);
        }

        [Authorize(Policy = Policies.Read)]
        [HttpGet("{id}/products")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByCategoryId(int id, int page = 0, int limit = 50)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryDto == null)
                return BadRequest();

            var dTOs = await _productService.GetAllProductsByCategoryIdAsync(id, page, limit);
            var models = dTOs.Select(ProductModel.MapFrom).ToList();

            return Ok(models);
        }

        // POST api/<CategoryController>
        [Authorize(Policy = Policies.Create)]
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] CategoryWriteModel model)
        {
            var dto = CategoryWriteModel.MapTo(model);
           
            CategoryDTO addedDto = await _categoryService.AddCategoryAsync(dto);           

            var addedModel = CategoryModel.MapFrom(addedDto);
            addedModel.Links = CreateLinksForCategory(addedModel.Id).ToList();

            return Created(nameof(GetById), addedModel);
        }

        // PUT api/<CategoryController>/5
        [Authorize(Policy = Policies.Update)]
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryWriteModel model)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryDto == null)
                return NotFound();

            var dto = CategoryWriteModel.MapTo(model);

            dto.Id = id;
            
            await _categoryService.UpdateCategoryAsync(dto);
            
            return await GetById(id);
        }

        // DELETE api/<CategoryController>/5
        [Authorize(Policy = Policies.Delete)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return NoContent();
        }


        private IEnumerable<Link> CreateLinksForCategory(int id)
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetById), values: new { id }) ?? "",
                "self",
                "GET"),

                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Delete), values: new { id })?? "",
                "delete_category",
                "DELETE"),

                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Update), values: new { id }) ?? "",
                "update_category",
                "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<CategoryModel> CreateLinksForGetAllCategories(LinkCollectionWrapper<CategoryModel> categoryWrapper)
        {
            categoryWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(Get), values: new { }) ?? "",
                    "self",
                    "GET"));

            return categoryWrapper;
        }
    }
}
