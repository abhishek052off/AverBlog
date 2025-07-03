using AverBlog.Business.IServices;
using AverBlog.Business.Services;
using AverBlog.Data;
using AverBlog.Data.Contexts;
using AverBlog.Data.IRepository;
using AverBlog.Data.Repository;
using Microsoft.EntityFrameworkCore;

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
                builder.Services.AddSwaggerGen();

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("AverBlog.Data")));

                builder.Services.AddScoped<IUserService , UserService>();

                builder.Services.AddScoped<IUserRepsitory ,UserRepsitory>();
            }

            var app = builder.Build();
            {
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
}
