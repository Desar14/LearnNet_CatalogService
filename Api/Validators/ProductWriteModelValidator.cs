using FluentValidation;
using LearnNet_CatalogService.Api.Models.Product;

namespace LearnNet_CatalogService.Api.Validators
{
    public class ProductWriteModelValidator : AbstractValidator<ProductWriteModel>
    {
        public ProductWriteModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
        }
    }
}
