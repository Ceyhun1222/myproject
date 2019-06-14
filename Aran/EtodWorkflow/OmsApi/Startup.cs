using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OmsApi.Data;
using OmsApi.Entity;
using OmsApi.Configuration;
using OmsApi.Interfaces;
using OmsApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json.Converters;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using Newtonsoft.Json;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetTopologySuite.Features;
using NetTopologySuite;
using Microsoft.AspNetCore.Mvc.Cors.Internal;

namespace OmsApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

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
                    Configuration.GetConnectionString(ConfigKeys.DefaultConnection)));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddTransient<IOmsEmailSender, MessageSender>();
            services.AddTransient<ISmsSender, MessageSender>();
            services.AddScoped<ISignInManager, AuditableSignInManager>();
            services.AddSingleton<ITokenService, TokenService>();
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddAutoMapper();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(600),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration[ConfigKeys.Domain4Client],
                        ValidAudience = Configuration[ConfigKeys.Domain4Client],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[ConfigKeys.SecurityKey]))
                    };
                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnAuthenticationFailed = context =>
                    //    {
                    //        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    //        {
                    //            context.Response.Headers.Add("Token-Expired", "true");
                    //        }

                    //        return Task.CompletedTask;
                    //    }
                    //};
                });
            //services.AddMvc(opt=>opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "OMS API",
                    Version = "ver. 1.0",
                    Description = "Obstacle Management System API",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();

                c.DescribeAllEnumsAsStrings();
            });

            services.UseConfigurationValidation();
            services.ConfigureValidatableSetting<SendGridConfig>(Configuration.GetSection(ConfigKeys.SendGrid));
            services.ConfigureValidatableSetting<MessageTemplateConfig>(Configuration.GetSection(ConfigKeys.MessageTemplateSettings));
            services.ConfigureValidatableSetting<AdminConfig>(Configuration.GetSection(ConfigKeys.Admin));
            services.ConfigureValidatableSetting<TossConfig>(Configuration.GetSection(ConfigKeys.Toss));
            services.ConfigureValidatableSetting<OmegaServiceConfig>(Configuration.GetSection(ConfigKeys.OmegaService));

            services.AddHttpClient<ITossClient, TossClient>().SetHandlerLifetime(TimeSpan.FromMinutes(10)).AddPolicyHandler(GetRetryPolicy());
            services.AddHttpClient<IOmegaServiceClient, OmegaServiceClient>().SetHandlerLifetime(TimeSpan.FromMinutes(10)).AddPolicyHandler(GetRetryPolicy());
            


            services.AddMvc().AddJsonOptions(options =>
            {
                //options.AllowInputFormatterExceptionMessages = true;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                //foreach (var converter in GeoJsonSerializer.Create(new JsonSerializerSettings(), NtsGeometryServices.Instance.CreateGeometryFactory(4326)).Converters)
                //{
                //    options.SerializerSettings.Converters.Add(converter);
                //}
                options.SerializerSettings.Error += (obj, args) =>
                {
                    var k = obj;

                };
                //options.SerializerSettings.Converters.Add(new NetTopologySuite.IO.Converters.CoordinateConverter());
                //options.SerializerSettings.Converters.Add(new NetTopologySuite.IO.Converters.GeometryArrayConverter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            IOptions<AdminConfig> adminSettings, ApplicationDbContext applicationDbContext)
        {
            app.UseCors("AllowAllHeaders");
            if (env.IsProduction())
            {
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OMS API ver.1");
                c.RoutePrefix = string.Empty;
            });

            //app.UseHttpsRedirection();
            //app.UseCookiePolicy();

            app.UseAuthentication();
            MyIdentityDataInitializer.SeedDataAsync(userManager, roleManager, Configuration,
                adminSettings.Value, applicationDbContext);

            app.UseMvc();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}
