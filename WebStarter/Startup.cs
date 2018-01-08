using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawler;
using EntityFrameworkCore.BootKit;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quickflow.Core;

namespace WebStarter
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment appEnv)
        {
            Configuration = configuration;
            CurrentEnvironment = appEnv;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string db = Configuration.GetSection("Database:Default").Value;
            string connection = Configuration.GetSection("Database:ConnectionStrings")[db];
            services.AddHangfire(x => x.UseSqlServerStorage(connection));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            Database.Assemblies = new String[] { "Quickflow.Core", "Quickflow.ActivityRepository", "Crawler" };
            Database.ContentRootPath = env.ContentRootPath;
            Database.Configuration = Configuration;

            DbInitWorkflows.Init();

            //app.UseHangfireServer();
            //app.UseHangfireDashboard("/schedulers");

            //RecurringJob.AddOrUpdate(() => CurrencyMarketRefreshJob.Refresh(0), Cron.MinuteInterval(5));
            CurrencyCrawler.Start();
            CurrencyMarketCrawler.Start();
        }
    }
}
