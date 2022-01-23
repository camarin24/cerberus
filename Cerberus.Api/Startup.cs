using AutoMapper;
using Cerberus.Api.Extensions;
using Cerberus.Api.Filters;
using Cerberus.Domain.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cerberus.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var mapConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
        var mapper = mapConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddControllers(cfg => { cfg.Filters.Add(typeof(AppExceptionFilter)); });

        services.AddAppStore();

        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Cerberus.Api", Version = "v1"}); });
        services.AddSingleton(_ => Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cerberus.Api v1"));
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}