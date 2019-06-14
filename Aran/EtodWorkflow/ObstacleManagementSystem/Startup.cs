using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ObstacleManagementSystem.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ObstacleManagementSystem.Models;
using ObstacleManagementSystem.Services;

namespace ObstacleManagementSystem
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<HtmlEncoder>(
     HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All,
                    UnicodeRanges.All }));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, MessageSender>();
            services.AddTransient<ISmsSender, MessageSender>();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc(opt => opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(ObstacleManagementSystem.Resources.Common));
                })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add(LanguageRouteConstraint.LangCode, typeof(LanguageRouteConstraint));
            });

            //services.AddSession();
            services.AddTransient<CustomLocalizer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/en/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            MyIdentityDataInitializer.SeedDataAsync(userManager, roleManager, Configuration);

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            LocalizationPipeline.ConfigureOptions(options.Value);
            app.UseRequestLocalization(options.Value);

        
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    //name:"default",
                    //template: "{area=admin}/{controller=account}/{action=login}/{id?}");
                    name: "area",
                    template: "{area:exists}/{controller=account}/{action=login}/{id?}");


                routes.MapRoute(
                    name: "default",
                    //name: "default2",
                    template: "{lang:" + LanguageRouteConstraint.LangCode + "}/" +
                              "{controller=Account}/{action=Login}/{id?}");
                routes.MapGet("{lang:lang}/{*path}", appbuilder =>
                {
                    //var defaultCulture = options.Value.DefaultRequestCulture.Culture.Name;
                    //var path = ctx.GetRouteValue("path") ?? string.Empty;
                    ////var culturedPath = $"/{defaultCulture}/{path}";
                    //var culturedPath = $"{ctx.Request.PathBase}/{defaultCulture}/{path}";
                    //ctx.Response.Redirect(culturedPath);
                    appbuilder.Response.Redirect("/en/home/error404");
                    return Task.CompletedTask;
                });
                routes.MapGet("{*path}", (RequestDelegate)(ctx =>
                {
                    var defaultCulture = options.Value.DefaultRequestCulture.Culture.Name;
                    var path = ctx.GetRouteValue("path") ?? string.Empty;
                    //var culturedPath = $"/{defaultCulture}/{path}";
                    var culturedPath = $"{ctx.Request.PathBase}/{defaultCulture}/{path}";
                    ctx.Response.Redirect(culturedPath);
                    return Task.CompletedTask;
                }));
            });
        }
    }
}
