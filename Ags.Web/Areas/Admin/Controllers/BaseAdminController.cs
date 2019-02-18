using System;
using System.Collections.Generic;
using System.Net;
using Ags.Data.Core;
using Ags.Data.Domain;
using Ags.Data.Domain.Customers;
using Ags.Services;
using Ags.Services.Customers;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework;
using Ags.Web.Framework.Controllers;
using Ags.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ags.Web.Areas.Admin.Controllers
{


    //[Authorize(Roles = "Admin,Editor")]
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AdminAntiForgery]
    [ValidateEditor]
    public abstract partial class BaseAdminController : BaseController
    {

        protected void AddBreadcrumb(string displayName, string urlPath)
        {
            List<Message> messages;

            if (ViewBag.Breadcrumb == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.Breadcrumb as List<Message>;
            }

            messages.Add(new Message { DisplayName = displayName, URLPath = urlPath });
            ViewBag.Breadcrumb = messages;
        }
        protected Customer GetCurrentUserAsync()
        {
            //var result = ServiceProviderFactory.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>().GetUserAsync(HttpContext.User);
            Customer customer = HttpContext.User.GetCustomer(
                ServiceProviderFactory.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(),
                ServiceProviderFactory.ServiceProvider.GetRequiredService<ICustomerService>());
            return customer;
        }
        protected void AddPageHeader(string pageHeader = "", string pageDescription = "")
        {
            ViewBag.PageHeader = Tuple.Create(pageHeader, pageDescription);
        }
        /// <summary>
        /// Save selected tab name
        /// </summary>
        /// <param name="tabName">Tab name to save; empty to automatically detect it</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request. Pass null to ignore</param>
        protected virtual void SaveSelectedTabName(string tabName = "", bool persistForTheNextRequest = true)
        {
            //default root tab
            SaveSelectedTabName(tabName, "selected-tab-name", null, persistForTheNextRequest);
            //child tabs (usually used for localization)
            //Form is available for POST only
            if (!Request.Method.Equals(WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("selected-tab-name-", StringComparison.InvariantCultureIgnoreCase))
                {
                    SaveSelectedTabName(null, key, key.Substring("selected-tab-name-".Length), persistForTheNextRequest);
                }
            }
        }

        /// <summary>
        /// Save selected tab name
        /// </summary>
        /// <param name="tabName">Tab name to save; empty to automatically detect it</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request. Pass null to ignore</param>
        /// <param name="formKey">Form key where selected tab name is stored</param>
        /// <param name="dataKeyPrefix">A prefix for child tab to process</param>
        protected virtual void SaveSelectedTabName(string tabName, string formKey, string dataKeyPrefix, bool persistForTheNextRequest)
        {
            //keep this method synchronized with
            //"GetSelectedTabName" method of \Nop.Web.Framework\Extensions\HtmlExtensions.cs
            if (string.IsNullOrEmpty(tabName))
            {
                tabName = Request.Form[formKey];
            }

            if (string.IsNullOrEmpty(tabName))
            {
                return;
            }

            string dataKey = "ags.selected-tab-name";
            if (!string.IsNullOrEmpty(dataKeyPrefix))
            {
                dataKey += $"-{dataKeyPrefix}";
            }

            if (persistForTheNextRequest)
            {
                TempData[dataKey] = tabName;
            }
            else
            {
                ViewData[dataKey] = tabName;
            }
        }/// <summary>
         /// Creates an object that serializes the specified object to JSON.
         /// </summary>
         /// <param name="data">The object to serialize.</param>
         /// <returns>The created object that serializes the specified data to JSON format for the response.</returns>
        public override JsonResult Json(object data)
        {
            //use IsoDateFormat on writing JSON text to fix issue with dates in KendoUI grid
            bool useIsoDateFormat = ServiceProviderFactory.ServiceProvider.GetRequiredService<StoreInformationSettings>()?.UseIsoDateFormatInJsonResult ?? false;
            JsonSerializerSettings serializerSettings = ServiceProviderFactory.ServiceProvider.GetRequiredService<IOptions<MvcJsonOptions>>()?.Value?.SerializerSettings
                ?? new JsonSerializerSettings();

            if (!useIsoDateFormat)
            {
                return base.Json(data, serializerSettings);
            }

            serializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;

            return base.Json(data, serializerSettings);
        }

    }
}