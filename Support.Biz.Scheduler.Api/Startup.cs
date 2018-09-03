using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Support.Biz.Scheduler.Repository;
using Support.Biz.Scheduler.Core;
using Support.Biz.Scheduler.Services;

namespace Support.Biz.Scheduler.Api
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
            services.AddMvc();
            services.AddTransient<IShiftService, ShiftService>();
            services.AddSingleton<IEngineersRepository, EngineersRepository>();
            services.AddSingleton<ISchedulerRepository, SchedulerRepository>();
            services.AddSingleton<IShiftRepository, ShiftRepository>();
            services.Configure<AppSettingsConfig>(Configuration.GetSection("DbContext"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:4100").AllowAnyMethod());

            app.UseMvc();
        }
    }
}
