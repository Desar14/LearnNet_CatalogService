using FluentValidation;
using LearnNet_CatalogService.Data.Entities;

namespace LearnNet_CatalogService.Domain.Validators
{
    public class CategoryValidator : AbstractValidator<Category<int>>
    {
        public CategoryValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}
