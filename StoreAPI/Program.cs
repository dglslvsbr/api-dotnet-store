using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreAPI.AppContext;
using StoreAPI.Customs;
using StoreAPI.Exceptions;
using StoreAPI.Interfaces;
using StoreAPI.Repositories;
using StoreAPI.Services;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace StoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Variables
            var configuration = builder.Configuration;
            var myAllowSpecifOrigin = "http://127.0.0.1:5500";

            // To suppress the automatic model state validation
            builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<CustomValidationFilter>();
                options.Filters.Add<GlobalException>();
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
            .AddNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Implemented Rate Limiting
            builder.Services.AddRateLimiter(options => options.AddFixedWindowLimiter("RateLimiter", options =>
            {
                options.PermitLimit = 1;
                options.Window = TimeSpan.FromSeconds(5);
                options.QueueLimit = 0;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            }));

            // Implemented CORS
            builder.Services.AddCors(options => options.AddPolicy("AllowCors", options => options.WithOrigins(myAllowSpecifOrigin).AllowAnyHeader().AllowAnyMethod()));

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
            builder.Services.AddDbContext<AppDbContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Time Life scoped
            builder.Services.AddScoped<ITokenService, TokenJwtService>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<AppDbContext>();
            builder.Services.AddScoped(typeof(ISystemCache<>), typeof(MemoryCache<>));

            // Auto Mapper
            builder.Services.AddAutoMapper(typeof(Mappings.AutoMapper));

            // Add Memory Cache
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseRateLimiter();

            app.UseCors(myAllowSpecifOrigin);

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}