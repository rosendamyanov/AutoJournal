using AutoJournal.Authentication.Configuration;
using AutoJournal.Authentication.Services;
using AutoJournal.Authentication.Services.Interfaces;
using AutoJournal.Authentication.Factory;
using AutoJournal.Authentication.Factory.Interfaces;
using AutoJournal.Authentication.Validation;
using AutoJournal.Authentication.Validation.Interfaces;
using AutoJournal.Services.Services;
using AutoJournal.Data.Context;
using AutoJournal.Data.Repositories;
using AutoJournal.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.OpenApi.Models;
namespace AutoJournal.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure DbContext
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            });

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            // Add services to the container.
            // Repositories
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();


            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            // Factories
            builder.Services.AddScoped<IAuthFactory, AuthFactory>();

            // Validators
            builder.Services.AddScoped<IEmailValidation, EmailValidation>();
            builder.Services.AddScoped<IPasswordValidation, PasswordValidation>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
            });

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
