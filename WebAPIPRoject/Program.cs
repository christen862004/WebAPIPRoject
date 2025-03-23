
using Microsoft.EntityFrameworkCore;
using WebAPIPRoject.Models;

namespace WebAPIPRoject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ITIContext>(option => {
                option.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())//for test
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseStaticFiles(); read image  --->
            //app.UseRoute();
            app.UseAuthorization();
            app.MapControllers();
            //mvc default route -web api  Resource
            //==>uniform name using route attribute[]

            app.Run();
        }
    }
}
