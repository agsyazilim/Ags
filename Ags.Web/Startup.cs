using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Ags.Data;
using Ags.Data.Core;
using Ags.Data.Core.Http;
using Ags.Data.Domain;
using Ags.Data.Domain.Logging;
using Ags.Services;
using Ags.Services.Logging;
using Ags.Web.Areas.Admin.Infrastructure.Mappers;
using Ags.Web.Framework.Infrastructure;
using Ags.Web.Framework.Mvc.ModelBinding;
using Ags.Web.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Ags.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
           // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.ReplaceService<ILazyLoader, AgsLazyLoader>();
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            });
            services.AgsAddHttpSession();
            services.AddOptions();
            services.AddResponseCompression();
            services.AgsAddAntiForgery();
            services.AgsAddTransist();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;

                }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Home/Error/{0}";
                options.Cookie.Name = $"{AgsCookieDefaults.Prefix}{AgsCookieDefaults.AuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/LogOff";
                // ReturnUrlParameter requires
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.Cookie.SecurePolicy = ServiceProviderFactory.ServiceProvider
                    .GetRequiredService<SecuritySettings>().ForceSslForAllPages
                    ? CookieSecurePolicy.SameAsRequest
                    : CookieSecurePolicy.None;
                options.SlidingExpiration = true;
            });
            //services.AgsAddAuthentication();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services
                .AddMvc().AddMvcOptions(options =>
                {
                    options.ModelMetadataDetailsProviders.Add(new AgsMetadataProvider());
                    options.ModelBinderProviders.Insert(0, new AgsModelBinderProvider());
                }).AddCookieTempDataProvider(options =>
                {
                    options.Cookie.Name = $"{AgsCookieDefaults.Prefix}{AgsCookieDefaults.TempDataCookie}";
                    options.Cookie.SecurePolicy = ServiceProviderFactory.ServiceProvider
                        .GetRequiredService<SecuritySettings>().ForceSslForAllPages
                        ? CookieSecurePolicy.SameAsRequest
                        : CookieSecurePolicy.None;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                // Maintain property names during serialization. See:
                // https://github.com/aspnet/Announcements/issues/194
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            Mapper.Initialize(cfg => cfg.AddProfile<AdminMapperConfiguration>());
            services.AddAutoMapper();
            ServiceProviderFactory.ServiceProvider = services.BuildServiceProvider();

            ServiceProviderFactory.ServiceProvider.GetRequiredService<ILogger>()
                .InsertLog(LogLevel.Information, "Application Started", null, null);
            services.AddEntityFrameworkSqlServer();
            services.AddEntityFrameworkProxies();
            services.AddRouting();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMiddleware(typeof(AuthenticationMiddleware));
            app.UseMiddleware(typeof(KeepAliveMiddleware));
            app.UseMiddleware(typeof(VisitorCounterMiddleware));
            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            app.UseExceptionHandler(handler =>
            {
                handler.Run(context =>
                {
                    System.Exception exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                    {
                        return Task.CompletedTask;
                    }
                    try
                    {
                        ServiceProviderFactory.ServiceProvider.GetService<ILogger>()
                            .Information(exception.Message, exception);
                    }
                    finally
                    {
                        throw exception;
                    }
                });
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("HomePage",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("Topic", "{SeName}",
                    new {controller = "Topic", action = "TopicDetails"});
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.Use(async (context, next) => { await next.Invoke(); });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("ill be executed after the code above!");
                Debug.WriteLine("invoke thru await next.Invoke();");
            });
        }
    }
}
