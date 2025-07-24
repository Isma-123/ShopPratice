
using Microsoft.EntityFrameworkCore;
using StoryDataBase.cs.Context;

namespace StoryApiWeb.cs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.  

            builder.Services.AddDbContext<StoryContext>(options => options.
            UseSqlServer(builder.Configuration.GetConnectionString("StoryDb")));


            // contenedor de dependencias
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
