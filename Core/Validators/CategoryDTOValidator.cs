﻿using FluentValidation;
using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Core.Validators
{
    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}