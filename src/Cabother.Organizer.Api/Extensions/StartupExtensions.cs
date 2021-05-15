using Cabother.Organizer.Api.Filters;
using Cabother.Organizer.Api.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Cabother.Organizer.Api.Extensions
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Adiciona a documentação swagger ao projeto
        /// </summary>
        /// <param name="services">Collection de injeção de dependências</param>
        public static void AddDocumentations(this IServiceCollection services)
        {
            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(2, 0);
                p.ReportApiVersions = true;
                p.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }

        /// <summary>
        /// Adiciona os filtros para injeção de dependência
        /// </summary>
        /// <param name="services">Objeto para configuração da injeção de dependência</param>
        public static void AddApiFilters(this IServiceCollection services)
        {
            services.AddScoped<ApplicationExceptionFilterAttribute>();
        }

        public static void UseDocumentations(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = "api/docs";
                options.DocExpansion(DocExpansion.List);
            });
        }
    }
}