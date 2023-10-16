using FluentValidation;
using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.Core.Interfaces;
using LearnNet_CatalogService.Core.Validators;
using LearnNet_CatalogService.Data.Entities;
using LearnNet_CatalogService.DataAccessSQL;
using LearnNet_CatalogService.Domain.Services;
using LearnNet_CatalogService.Domain.Validators;
using Microsoft.EntityFrameworkCore;

namespace LearnNet_CatalogService
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

            builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
            builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
            builder.Services.AddScoped<IValidator<CategoryDTO>, CategoryDTOValidator>();
            builder.Services.AddScoped<IValidator<ProductDTO>, ProductDTOValidator>();
            builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();
            builder.Services.AddScoped<IValidator<Product>, ProductValidator>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();

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