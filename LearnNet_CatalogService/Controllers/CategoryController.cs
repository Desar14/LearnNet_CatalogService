using FluentValidation;
using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.Core.Interfaces;
using LearnNet_CatalogService.HATEOAS;
using LearnNet_CatalogService.Models.Category;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LearnNet_CatalogService.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly IValidator<CategoryDTO> _validator;

        public CategoryController(ICategoryService categoryService,
                                  ILogger<CategoryController> logger,
                                  LinkGenerator linkGenerator,
                                  IValidator<CategoryDTO> validator)
        {
            _categoryService = categoryService;
            _logger = logger;
            _linkGenerator = linkGenerator;
            _validator = validator;
        }

        // GET: api/<CategoryController>
        [HttpGet]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryDto == null)
                return NotFound();

            var model = CategoryModel.MapFrom(categoryDto);
            model.Links = CreateLinksForCategory(model.Id).ToList();

            return Ok(model);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryWriteModel model)
        {
            if (model == null)
                return BadRequest();

            var dto = CategoryWriteModel.MapTo(model);

            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid) 
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            int addedId = -1;

            try
            {
                addedId = await _categoryService.AddCategoryAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Add category error");
                throw;
            }

            if (addedId == -1)
            {
                return StatusCode(500);
            }

            return await GetById(addedId);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryWriteModel model)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryDto == null)
                return NotFound();

            var dto = CategoryWriteModel.MapTo(model);

            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            bool addedSuccess = false;

            dto.Id = id;

            try
            {
                addedSuccess = await _categoryService.UpdateCategoryAsync(dto);
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

            return await GetById(id);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryDto == null)
                return NotFound();

            var result = await _categoryService.DeleteCategoryAsync(id);

            if (!result)
            {
                return StatusCode(500);
            }

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
