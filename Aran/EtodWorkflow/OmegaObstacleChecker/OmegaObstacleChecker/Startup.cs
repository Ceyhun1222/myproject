using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ObstacleChecker.API.Extensions;
using ObstacleChecker.API.Model;
using ObstacleChecker.API.Utils;
using Serilog;
using Serilog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace ObstacleChecker.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var baseAdress = Configuration["TossWebApiAddress"];
            services.AddTransient<ITossHttpClient,TossHttpClient>(tossClient => new TossHttpClient(baseAdress));

            services.AddTransient<IMathTranformCreater,MathTransformCreater>();

            services.AddSingleton(Log.Logger);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Omega Obstacle Checker Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Omega Obstacle Checker API V1");
            });

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
