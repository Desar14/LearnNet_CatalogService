using FluentValidation;
using LearnNet_CatalogService.Api.Models.Category;
using LearnNet_CatalogService.Api.Models.Product;
using LearnNet_CatalogService.Api.Validators;
using LearnNet_CatalogService.Core.Interfaces;
using LearnNet_CatalogService.Data.Entities;
using LearnNet_CatalogService.DataAccessSQL;
using LearnNet_CatalogService.Domain.Services;
using LearnNet_CatalogService.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace LearnNet_CatalogService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IRepository<Category<int>, int>, Repository<Category<int>, int>>();
            builder.Services.AddScoped<IRepository<Product<int>, int>, Repository<Product<int>, int>>();
            builder.Services.AddScoped<IValidator<CategoryWriteModel>, CategoryWriteModelValidator>();
            builder.Services.AddScoped<IValidator<ProductWriteModel>, ProductWriteModelValidator>();
            builder.Services.AddScoped<IValidator<Category<int>>, CategoryValidator>();
            builder.Services.AddScoped<IValidator<Product<int>>, ProductValidator>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddFluentValidationAutoValidation();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}