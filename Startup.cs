using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.OpenApi.Models;
using Polly;

namespace HelloDotnet5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HelloDotnet5", Version = "v1" });
            });

            services.Configure<ServiceSettings>(Configuration.GetSection(nameof(ServiceSettings)));
            services.AddHttpClient<WeatherClient>()
            // Adding policy for request error due disponibility
            // Exponential backoff --> It will be retry 10 times, and in each retry, it is going to sleep how much the expression specifies i.e. 2 power 1... 2 power 2 = 4... 2 power 3 = 8
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
            // Circuit Breaker --> Allows to break the requests after the attempts (3 in this case)
            .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(10)));

            // Enabling health checks to the service
            services.AddHealthChecks()
            // Adding health check implementation (you can add ho many as you need)
            .AddCheck<ExternalEndpointHealthCheck>("OpenWeather");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HelloDotnet5 v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Setting the routes to service heatlh checking
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
