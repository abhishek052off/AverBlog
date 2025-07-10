using AverBlog.Api.CustomMiddlewares;
using AverBlog.Api.SessionModels;
using AverBlog.Business.IServices;
using AverBlog.Business.Services;
using AverBlog.Data;
using AverBlog.Data.Contexts;
using AverBlog.Data.IRepository;
using AverBlog.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace AverBlog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            {
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AverBlog API", Version = "v1" });

                    
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter The Token Here"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                });


                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("AverBlog.Data")));


                var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];
                var key = jwtSettings["Key"];

                builder.Services.AddAuthentication(
                    options =>
                        {
                            options.DefaultAuthenticateScheme =  JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        }
                    ).AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = issuer,
                            ValidAudience = audience,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(key))
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Console.WriteLine($"JWT error: {context.Exception.Message}");
                                return Task.CompletedTask;
                            }
                        };
                    });

                builder.Services.AddAuthorization(
                    options =>
                    {
                        options.AddPolicy("AdminOnlyPolicy", policy=> policy.RequireClaim(ClaimTypes.Role , "Admin"));
                        options.AddPolicy("AdminAndSales", policy=> policy.RequireAssertion( context=> context.User.IsInRole("Admin") || context.User.IsInRole("Sales") ) );
                        
                    });

                builder.Services.AddScoped<IUserService , UserService>();

                builder.Services.AddScoped<IUserRepsitory ,UserRepsitory>();
                builder.Services.AddScoped<UserSession>();
            }

            var app = builder.Build();
            {
                

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }


                app.UseMiddleware<GlobalExceptionHandlerMiddleWare>();
                //do somethng here 

                app.UseHttpsRedirection();

                app.UseAuthentication();
                
                app.UseAuthorization();
                app.UseMiddleware<UserSessionMiddleWare>();

                app.MapControllers();

                app.Run();
            }

        }
    }
}
