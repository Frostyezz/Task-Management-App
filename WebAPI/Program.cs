
using DataAccessLayer.Repository;
using DataAccessLayer;
using BusinessLayer.Contracts;
using BusinessLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Repository
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //Services
            builder.Services.AddScoped(typeof(ITestService), typeof(TestService));

            //Database
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext")));
                var conf = builder.Configuration;
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = conf["Jwt:Issuer"].ToString(),
                        ValidAudience = conf["Jwt:Audience"].ToString(),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]))
                    };
                });

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("Default", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
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

            app.UseCors("Default");

            app.MapControllers();

            app.Run();
        }
    }
}
