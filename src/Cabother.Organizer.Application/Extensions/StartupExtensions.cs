using AutoMapper;
using Cabother.Organizer.Application.AutoMappers;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cabother.Organizer.Application.Extensions
{
    public static class StartupExtensions
    {
        public static void AddMediator(this IServiceCollection services)
        {
            var assembly = typeof(StartupExtensions).Assembly;
            services.AddMediatR(assembly);
            services.AddFluentValidation(new[] { assembly });
        }
        public static void AddApplicationMappers(this IMapperConfigurationExpression config)
        {
            config.AddProfile<TeamProfile>();
        }
    }
}