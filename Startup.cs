using Gevlee.RestTunes.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gevlee.RestTunes
{
    public class Startup
    {
        private readonly ILoggerFactory loggerFactory;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            this.loggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsAny", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddResponseCaching();
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            })
            .AddNewtonsoftJson()
            .AddXmlSerializerFormatters();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                /*
                 *  UrlSegmentApiVersionReader – Enable Url Versioning
                    HeaderApiVersionReader – Enable Header Versioning
                    QueryStringApiVersionReader – Enable Query String Versioning
                    MediaTypeApiVersionReader – Enable Mediatype Versioning
                 */
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionSelector = new LowestImplementedApiVersionSelector(options);
                options.ReportApiVersions = true;
            });

            services.AddSingleton<IActionContextAccessor>(new ActionContextAccessor());
            services.AddScoped(x =>
                x.GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            services.AddDbContext<TunesContext>(builder =>
                builder.UseSqlite("Datasource=db.sqlite")
                .UseLoggerFactory(loggerFactory)
            );

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen();
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddSwaggerGenNewtonsoftSupport();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsAny");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            app.UseResponseCaching();
            app.UseRouting().UseEndpoints(c =>
            {
                c.MapControllers();
            });
        }
    }
}