using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzEX.Jobs;
using QuartzEX.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzEX
{
    public class Startup
    {
        private IScheduler _quartzScheduler;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _quartzScheduler = ConfigureQuartz();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddTransient<SingleJob>();
            services.AddSingleton(provider => _quartzScheduler);

        }

        private void OnShutdown()
        {
            if (!_quartzScheduler.IsShutdown) _quartzScheduler.Shutdown() ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            _quartzScheduler.JobFactory = new AspNetCoreJobFactory(app.ApplicationServices);
           
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public IScheduler ConfigureQuartz()
        {
            NameValueCollection props = new NameValueCollection {
                {"quartz.serializer.type", "binary" },
          
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            var scheduler = factory.GetScheduler().Result;
        
            scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
            scheduler.ListenerManager.AddJobListener(new JobListener());
            scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            return scheduler;

        }
    }
}
