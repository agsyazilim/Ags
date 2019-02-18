using System;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Common;
using Ags.Data.Core;
using Ags.Data.Core.Caching;
using Ags.Data.Core.Http;
using Ags.Data.Core.Repository;
using Ags.Data.Domain;
using Ags.Data.Domain.Seo;
using Ags.Services.Blogs;
using Ags.Services.Catalog;
using Ags.Services.Common;
using Ags.Services.Companys;
using Ags.Services.Configuration;
using Ags.Services.Customers;
using Ags.Services.Directory;
using Ags.Services.Events;
using Ags.Services.Helpers;
using Ags.Services.Logging;
using Ags.Services.Media;
using Ags.Services.Message;
using Ags.Services.News;
using Ags.Services.NewsPapers;
using Ags.Services.Polls;
using Ags.Services.Seo;
using Ags.Services.Stores;
using Ags.Services.Tasks;
using Ags.Services.Topics;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Factories;
using Ags.Web.Framework;
using Ags.Web.Framework.Authorization;
using Ags.Web.Framework.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;



namespace Ags.Web.Infrastructure
{
    /// <summary>
    /// ServiceCollectionExtensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// AgsAddTransist
        /// </summary>
        /// <param name="services"></param>
        public static void AgsAddTransist(this IServiceCollection services)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAgsFileProvider, AgsFileProvider>();
            services.AddTransient<IWebHelper, WebHelper>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
            services.AddScoped<SignInManager<ApplicationUser>, AuditableSignInManager<ApplicationUser>>();
            services.AddTransient<IDbContext, ApplicationDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient(x =>
                x.GetRequiredService<ISettingService>().LoadSetting<StoreInformationSettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<SeoSettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<BlogSettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<CaptchaSettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<CatalogSettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<MediaSettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<NewsSettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<SecuritySettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<CommonSettings>());
            services.AddTransient(x => x.GetRequiredService<ISettingService>().LoadSetting<EmailAccountSettings>());
            services.AddTransient<IPageHeadBuilder, PageHeadBuilder>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IPictureService, PictureService>();
            services.AddTransient<IUrlRecordService, UrlRecordService>();
            services.AddTransient<ISettingModelFactory, SettingModelFactory>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<ICacheManager, PerRequestCacheManager>();
            services.AddTransient<IStoreContext, WebStoreContext>();
            services.AddTransient<IUserAgentHelper, UserAgentHelper>();
            services.AddSingleton<ILocker, MemoryCacheManager>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
            services.AddTransient<ITopicTemplateService, TopicTemplateService>();
            services.AddTransient<ITopicService, TopicService>();
            services.AddTransient<IStateProvinceService, StateProvinceService>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IStoreContext, WebStoreContext>();
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IPictureService, PictureService>();
            services.AddTransient<IUrlRecordService, UrlRecordService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IPollService, PollService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICategoryTemplateService, CategoryTemplateService>();
            services.AddTransient<ISearchTermService, SearchTermService>();
            services.AddTransient<IGenericAttributeService, GenericAttributeService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IEventPublisher, EventPublisher>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IScheduleTaskService, ScheduleTaskService>();
            services.AddTransient<ILogger, DefaultLogger>();
            services.AddTransient<ICustomerActivityService, CustomerActivityService>();
            services.AddTransient<ISearchTermService, SearchTermService>();
            services.AddTransient<IGenericAttributeService, GenericAttributeService>();
            services.AddTransient<IEmailAccountService, EmailAccountService>();
            services.AddScoped<IAuthorizationHandler, CustomerAdministratorsAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, CustomerIsOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, CustomerManagerAuthorizationHandler>();
            services.AddTransient<IActivityLogModelFactory, ActivityLogModelFactory>();
            services.AddTransient<IBaseAdminModelFactory, BaseAdminModelFactory>();
            services.AddTransient<IBlogModelFactory, BlogModelFactory>();
            services.AddTransient<ICategoryModelFactory, CategoryModelFactory>();
            services.AddTransient<IEmailAccountModelFactory, EmailAccountModelFactory>();
            services.AddTransient<Areas.Admin.Factories.ICommonModelFactory, Areas.Admin.Factories.CommonModelFactory
                >();
            services.AddTransient<ILogModelFactory, LogModelFactory>();
            services.AddTransient<Areas.Admin.Factories.INewsModelFactory, Areas.Admin.Factories.NewsModelFactory>();
            services.AddTransient<Areas.Admin.Factories.IPollModelFactory, Areas.Admin.Factories.PollModelFactory>();
            services.AddTransient<Ags.Web.Factories.IPollModelFactory,Ags.Web.Factories.PollModelFactory>();
            services.AddTransient<IScheduleTaskModelFactory, ScheduleTaskModelFactory>();
            services.AddTransient<ISettingModelFactory, SettingModelFactory>();
            services.AddTransient<IStoreModelFactory, StoreModelFactory>();
            services.AddTransient<ITemplateModelFactory, TemplateModelFactory>();
            services.AddTransient<Areas.Admin.Factories.ITopicModelFactory, Areas.Admin.Factories.TopicModelFactory>();
            services.AddSingleton<ISubscriptionService, SubscriptionService>();
            services.AddTransient<INewsPaperServices, NewsPaperServices>();
            services.AddTransient<INewsPaperModelFactory, NewsPaperModelFactory>();
            services.AddTransient<ISectionService, SectionService>();
            services.AddTransient<Factories.ICommonModelFactory, Factories.CommonModelFactory>();
            services.AddTransient<Factories.ITopicModelFactory, Factories.TopicModelFactory>();
            services.AddTransient<Factories.INewsModelFactory, Factories.NewsModelFactory>();
            services.AddTransient<ICustomerBlogFactory, CustomerBlogFactory>();
            services.AddTransient<ICatalogModelFactory, CatalogModelFactory>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IVideoService, VideoService>();
            services.AddTransient<IGalleryService, GalleryService>();
            services.AddTransient<ISliderService, SliderService>();
            services.AddTransient<IVideoFactory, VideoFactory>();
            services.AddTransient<ISliderFactory, SliderFactory>();
            services.AddTransient<IGalleryFactory, GalleryFactory>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ICompanyFactoryModel, CompanyFactoryModel>();
            services.AddTransient<IVisitorCounterService, VisitorCounterService>();
            services.AddTransient<INewsCounterService, NewsCounterService>();
            services.AddTransient<IAdvertisementService, AdvertisementService>();

            //services.AddScoped<IUrlHelper>(provider =>
            //{
            //    var actionContex = provider.GetService<IActionContextAccessor>().ActionContext;
            //    return new UrlHelper(actionContex);
            //});


        }
        /// <summary>
        /// AgsAddAntiForgery
        /// </summary>
        /// <param name="services"></param>
        public static void AgsAddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{AgsCookieDefaults.Prefix}{AgsCookieDefaults.AntiforgeryCookie}";
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;

            });


        }
        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AgsAddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{AgsCookieDefaults.Prefix}{AgsCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;
                //whether to allow the use of session values from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            });
        }

        public static void AgsAddAuthentication(this IServiceCollection services)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddFacebook(options =>
                {
                    options.AppId = "287650545241253";
                    options.AppSecret = "63798ad9b98c052b90bc89113d79ba8d";
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/LogOff";
                    options.Cookie.Name = $"{AgsCookieDefaults.Prefix}{AgsCookieDefaults.AuthenticationCookie}";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Expiration = DateTime.Now.Subtract(DateTime.UtcNow).Add(TimeSpan.FromMinutes(5));
                    options.AccessDeniedPath = "/Home/Error/{0}";
                    options.Cookie.SecurePolicy = ServiceProviderFactory.ServiceProvider
                        .GetRequiredService<SecuritySettings>().ForceSslForAllPages
                        ? CookieSecurePolicy.SameAsRequest
                        : CookieSecurePolicy.None;
                    options.SlidingExpiration = true;
                });
        }

    }

}