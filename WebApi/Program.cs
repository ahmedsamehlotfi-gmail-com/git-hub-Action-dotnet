
using Application;
using Application.Features.Identity.Users.Commands;
using Application.Features.Tenancy;
using Infrastructure;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("D:\\Advancied_.Net\\Advanced .NET Web API Multi-Tenant Applications\\ABC_NewSchoolB\\Looger\\myapp-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationService();
            builder.Host.UseSerilog();
            var app = builder.Build();

            await app.Services.AddDatabaseInitializerAsync();

            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlingMiddleWare>(); 
            app.MapControllers();

            app.UseInfrastructure();
            app.UseSerilogRequestLogging();
            app.Run();
        }
    }
}
