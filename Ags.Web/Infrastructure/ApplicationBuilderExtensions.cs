// AgsAdminLTEAtilla Kaya18:44

using System;
using System.Threading.Tasks;
using Ags.Data.Core;
using Ags.Data.Core.Http;
using Ags.Data.Domain;
using Ags.Data.Domain.Customers;
using Ags.Services;
using Ags.Services.Customers;
using Ags.Services.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Ags.Web.Infrastructure
{
    /// <summary>
    /// ApplicationBuilderExtensions
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// UsePageNotFound
        /// </summary>
        /// <param name="application"></param>
        public static void UsePageNotFound(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(async context =>
            {
                //handle 404 Not Found
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var webHelper = ServiceProviderFactory.ServiceProvider.GetRequiredService<IWebHelper>();
                    if (!webHelper.IsStaticResource())
                    {
                        //get original path and query
                        var originalPath = context.HttpContext.Request.Path;
                        var originalQueryString = context.HttpContext.Request.QueryString;

                        //store the original paths in special feature, so we can use it later
                        context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
                        {
                            OriginalPathBase = context.HttpContext.Request.PathBase.Value,
                            OriginalPath = originalPath.Value,
                            OriginalQueryString = originalQueryString.HasValue ? originalQueryString.Value : null,
                        });

                        //get new path
                        context.HttpContext.Request.Path = "/Common/page-not-found";
                        context.HttpContext.Request.QueryString = QueryString.Empty;

                        try
                        {
                            //re-execute request with new path
                            await context.Next(context.HttpContext);
                        }
                        finally
                        {
                            //return original path to request
                            context.HttpContext.Request.QueryString = originalQueryString;
                            context.HttpContext.Request.Path = originalPath;
                            context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(null);
                        }
                    }
                }
            });
        }
        /// <summary>
        /// UseBadRequestResult
        /// </summary>
        /// <param name="application"></param>
        public static void UseBadRequestResult(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(context =>
            {
                    //handle 404 (Bad request)
                    if (context.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    var logger = ServiceProviderFactory.ServiceProvider.GetRequiredService<ILogger>();
                    Customer workContext = null;
                    if (context.HttpContext.User.Identity.IsAuthenticated)
                    {
                        workContext = context.HttpContext.User.GetCustomer(
                                ServiceProviderFactory.ServiceProvider
                                    .GetRequiredService<UserManager<ApplicationUser>>(),
                                ServiceProviderFactory.ServiceProvider.GetRequiredService<ICustomerService>());

                        logger.Error("Error 400. Bad request", null, customer: workContext);
                    }
                    else
                    {
                        logger.Error("Error 400. Bad request", null);

                    }
                }

                return Task.CompletedTask;
            });
        }
        /// <summary>
        /// UseNopStaticFiles
        /// </summary>
        /// <param name="application"></param>
        public static void UseNopStaticFiles(this IApplicationBuilder application)
        {
           Action<StaticFileResponseContext> staticFileResponse = (context) =>
            {

                var commonSettings = ServiceProviderFactory.ServiceProvider.GetRequiredService<CommonSettings>();
                if (!string.IsNullOrEmpty(commonSettings.StaticFilesCacheControl))
                    context.Context.Response.Headers.Append(HeaderNames.CacheControl, commonSettings.StaticFilesCacheControl);

            };
            //common static files
            application.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = staticFileResponse });
        }
        public static void UseNopAuthentication(this IApplicationBuilder application)
        {

            application.UseMiddleware<AuthenticationMiddleware>();
        }
        public static void UseKeepAlive(this IApplicationBuilder application)
        {
            application.UseMiddleware<KeepAliveMiddleware>();
        }
    }
}