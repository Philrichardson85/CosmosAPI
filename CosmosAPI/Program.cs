using CosmosAPI.Models;
using CosmosAPI.Services;
using Microsoft.AspNetCore.OData;

namespace CosmosAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddOData(options =>
            options.Select().Filter().OrderBy()
            );
            builder.Services.AddTransient<ICosmosService, CosmosService>();

            var config = new ConfigurationBuilder()
                            .AddJsonFile("appSettings.json")
                            .Build();
            builder.Services.Configure<ConnectionStringsSection>(config.GetSection("ConnectionStrings"));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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