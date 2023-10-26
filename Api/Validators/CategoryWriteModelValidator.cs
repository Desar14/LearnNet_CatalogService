using FluentValidation;
using LearnNet_CatalogService.Api.Models.Category;

namespace LearnNet_CatalogService.Api.Validators
{
    public class CategoryWriteModelValidator : AbstractValidator<CategoryWriteModel>
    {
        public CategoryWriteModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}
