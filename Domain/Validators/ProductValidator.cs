using FluentValidation;
using LearnNet_CatalogService.Data.Entities;

namespace LearnNet_CatalogService.Domain.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Amount).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
