﻿using FluentValidation;
using LearnNet_CatalogService.Data.Entities;

namespace LearnNet_CatalogService.Domain.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
        }
    }
}
