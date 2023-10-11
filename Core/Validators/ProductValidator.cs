﻿using FluentValidation;
using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Domain.Validators
{
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Amount).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
