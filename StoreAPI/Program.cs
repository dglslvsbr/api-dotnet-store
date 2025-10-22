using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StoreAPI.Context;
using StoreAPI.Customs;
using StoreAPI.Filters;
using StoreAPI.Interfaces;
using StoreAPI.Middlewares;
using StoreAPI.Repositories;
using StoreAPI.Services;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace StoreAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Variables
        var configuration = builder.Configuration;
        var myAllowSpecifOrigin = new List<string>
        {
            "http://127.0.0.1:5500",
            "http://localhost:5500"
        };

        // To suppress the automatic model state validation
        builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        // Add services to the container.
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<CustomValidationFilter>();
            options.Filters.Add<GlobalExceptionFilter>();
        })
        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Store API",
                Version = "v1",
                Description = "RESTful API for user authentication, product management, and orders."
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        // Implemented Rate Limiting
        builder.Services.AddRateLimiter(options => options.AddFixedWindowLimiter("RateLimiter", options =>
        {
            options.PermitLimit = 5;
            options.Window = TimeSpan.FromSeconds(5);
            options.QueueLimit = 0;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        }));

        // Implemented CORS
        builder.Services.AddCors(options => options.AddPolicy("AllowCors", options =>
        options.WithOrigins(myAllowSpecifOrigin[0], myAllowSpecifOrigin[1])
        .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

        // Implemented authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            });

        // Implemented authorization
        builder.Services.AddAuthorizationBuilder().AddPolicy("UserOnly", options => options.RequireRole("User"))
                                                  .AddPolicy("AdminOnly", options => options.RequireRole("Admin"));

        // DbContext configured
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));

        // Time Life scoped
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

        builder.Services.AddScoped<ITokenService, TokenJwtService>();

        // Auto Mapper
        builder.Services.AddAutoMapper(typeof(Mappings.AutoMapper));

        // Add Memory Cache
        builder.Services.AddMemoryCache();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API v1");
            });
        }

        app.UseMiddleware<ResponseTimeMiddleware>();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowCors");

        app.UseRateLimiter();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying migrations: {ex.Message}");
        }
        app.Run();
    }
}