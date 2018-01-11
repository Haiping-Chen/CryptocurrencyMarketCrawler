using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawler;
using EntityFrameworkCore.BootKit;
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

            SchedulerInitializer.Initialize();

            //new CurrencyCrawler().Execute(null);
            //new CurrencyMarketCrawler().Execute(null);
        }
    }
}
