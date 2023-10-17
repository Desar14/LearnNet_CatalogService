using FluentValidation;
using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Core.Validators
{
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
        }
    }
}
