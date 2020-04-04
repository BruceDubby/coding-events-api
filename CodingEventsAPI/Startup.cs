using CodingEventsAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CodingEventsAPI {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers();

      // TODO: assign the connection string value from external configuration
      var connectionString = "";
      services.AddDbContext<CodingEventsDbContext>(o => o.UseMySql(connectionString));

      services.AddSwaggerGen(
        options => {
          options.SwaggerDoc(
            "v1",
            new OpenApiInfo {
              Version = "v1",
              Title = "Coding Events API",
              Description = "REST API for managing Coding Events"
            }
          );
        }
      );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

      app.UseSwagger();
      app.UseSwaggerUI(
        options => {
          options.SwaggerEndpoint("/swagger/v1/swagger.json", "Code Events API Documentation");
        }
      );

      // run migrations on startup
      var dbContext = app.ApplicationServices.CreateScope()
        .ServiceProvider.GetService<CodingEventsDbContext>();
      dbContext.Database.Migrate();
    }
  }
}