using Azure.Identity;
using FluentValidation;
using LearnNet_CatalogService.Api.Auth;
using LearnNet_CatalogService.Api.Models.Category;
using LearnNet_CatalogService.Api.Models.Product;
using LearnNet_CatalogService.Api.Validators;
using LearnNet_CatalogService.Core.Interfaces;
using LearnNet_CatalogService.Data.Entities;
using LearnNet_CatalogService.DataAccessSQL;
using LearnNet_CatalogService.Domain.Services;
using LearnNet_CatalogService.Domain.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;

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

            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["JWT:ValidIssuer"];

                options.TokenValidationParameters.ValidateAudience = true;
                options.Audience = builder.Configuration["JWT:ValidAudience"];

                // it's recommended to check the type header to avoid "JWT confusion" attacks
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Read, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(x => x.User.HasClaim(x => x.Type == "scope" && x.Value.Contains("read")));
                });

                options.AddPolicy(Policies.Create, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(x => x.User.HasClaim(x => x.Type == "scope" && x.Value.Contains("create")));
                });

                options.AddPolicy(Policies.Update, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(x => x.User.HasClaim(x => x.Type == "scope" && x.Value.Contains("update")));
                });

                options.AddPolicy(Policies.Delete, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(x => x.User.HasClaim(x => x.Type == "scope" && x.Value.Contains("delete")));
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddScoped<IRepository<Category, int>, Repository<Category, int>>();
            builder.Services.AddScoped<IRepository<Product, int>, Repository<Product, int>>();
            builder.Services.AddScoped<IValidator<CategoryWriteModel>, CategoryWriteModelValidator>();
            builder.Services.AddScoped<IValidator<ProductWriteModel>, ProductWriteModelValidator>();
            builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();
            builder.Services.AddScoped<IValidator<Product>, ProductValidator>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddAzureClients(clientsBuilder =>
            {
                clientsBuilder.AddServiceBusClientWithNamespace(builder.Configuration.GetSection("ServiceBus")["FullyQualifiedNamespace"])
                  // (Optional) Provide name for instance to retrieve by with DI
                  .WithName("ProductUpdates")
                  // (Optional) Override ServiceBusClientOptions (e.g. change retry settings)
                  .ConfigureOptions(options =>
                  {
                      options.RetryOptions.Delay = TimeSpan.FromMilliseconds(50);
                      options.RetryOptions.MaxDelay = TimeSpan.FromSeconds(5);
                      options.RetryOptions.MaxRetries = 3;
                  });
                clientsBuilder.UseCredential(new DefaultAzureCredential());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}