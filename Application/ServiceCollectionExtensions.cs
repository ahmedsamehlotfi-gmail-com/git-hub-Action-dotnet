using Application.Features.Identity.Users.Commands;
using Application.PipeLines;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return services
                .AddValidatorsFromAssembly(assembly)
                .AddTransient(typeof(IPipelineBehavior< , >) , typeof(ValidationPipeLineBehavior< , >))
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(assembly);
                });

        }
    }
}
