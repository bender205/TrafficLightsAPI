using System.Reflection;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrafficLights.Core.Hubs;
using TrafficLights.Data;
using TrafficLights.Data.DataAccess;
using TrafficLights.Model;
using TrafficLights.WorkerService;

namespace TrafficLights.Api
{

    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";//added this
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddDbContext<TraficLightsContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Transient);
            /*options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Transient);*/
            //   services.AddSingleton<TrafficLight>();
            //services.AddTransient<TrafficLight>();
            services.AddScoped<TrafficLight>();
            services.AddScoped<TrafficLightRepository>();
            //     services.AddScoped<TraficLightsWorker>();
            
            services.AddMediatR(Assembly.GetExecutingAssembly(), Assembly.Load(("TrafficLights.Core")));
            /*services.AddSingleton<IHostedService, Worker>();*/
            services.AddSingleton<TrafficLightsService>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowCredentials();
                                  });
            });//added this

            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddMvc();

            services.AddHostedService<Worker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(MyAllowSpecificOrigins);//added this
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=TrafficLights}/{action=Index}/{id?}");
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<TraficLightsHub>("/lighthub");
                });
            });
        }
    }
}
