using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using HomeAssigment.Models;
using HomeAssigment.Interface;
using HomeAssigment.DataAccessLayer;
using HomeAssigment.Services;
using Microsoft.AspNetCore.Hosting;
using Prometheus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;



namespace HomeAssigment
{
    public class Startup
    {
        #region Public members

        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Builds a Startup object.
        /// </summary>
        /// <param name="configuration">The object which contains the configuration settings.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Configures the services used by the application.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure contexts

            services.AddDbContext<CustomerDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("CustomerService")));

            // Configure services
            services.AddScoped<ICustomer, CustomerService>();
         
            // Add services to the container.
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "CustomerService.Web.Api", Version = "v1" });
            });

            services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddConsoleExporter() // Export traces to console
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CustomerServiceAPI"));
            });
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .WithExposedHeaders("Content-Disposition");
            }));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application object.</param>
        /// <param name="env">The environment object.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerService.Web.Api v1"));
            }
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            app.UseHttpsRedirection();

            // Enable Serilog Request Logging
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthorization();

            // Enable Prometheus metrics endpoint
            app.UseMetricServer(); // Exposes metrics at `/metrics`
            app.UseHttpMetrics();  // Tracks HTTP request metrics


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors("CorsPolicy");
        }

        #endregion
    }
}
