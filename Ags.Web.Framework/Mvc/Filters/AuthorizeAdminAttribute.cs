using System;
using System.Linq;
using Ags.Data.Domain;
using Ags.Services.Customers;
using Ags.Web.Framework.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ags.Web.Framework.Mvc.Filters
{
    /// <summary>
    /// Represents a filter attribute that confirms access to the admin panel
    /// </summary>
    public class AuthorizeAdminAttribute : TypeFilterAttribute
    {
        #region Fields

        private readonly bool _ignoreFilter;


        #endregion

        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        /// <param name="ignore">Whether to ignore the execution of filter actions</param>
        public AuthorizeAdminAttribute(bool ignore = false) : base(typeof(AuthorizeAdminFilter))
        {

            this._ignoreFilter = ignore;
            this.Arguments = new object[] { ignore };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether to ignore the execution of filter actions
        /// </summary>
        public bool IgnoreFilter => _ignoreFilter;


        #endregion


        #region Nested filter

        /// <summary>
        /// Represents a filter that confirms access to the admin panel
        /// </summary>
        private class AuthorizeAdminFilter : IAuthorizationFilter
        {
            #region Fields

            private readonly bool _ignoreFilter;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ICustomerService _customerService;
            private readonly UserManager<ApplicationUser> _appUserManager;

            #endregion

            #region Ctor

            public AuthorizeAdminFilter(bool ignoreFilter, IHttpContextAccessor httpContextAccessor, ICustomerService customerService, UserManager<ApplicationUser> appUserManager)
            {
                this._ignoreFilter = ignoreFilter;
                _httpContextAccessor = httpContextAccessor;
                _customerService = customerService;
                _appUserManager = appUserManager;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Called early in the filter pipeline to confirm request is authorized
            /// </summary>
            /// <param name="filterContext">Authorization filter context</param>
            public void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                if (filterContext == null)
                    throw new ArgumentNullException(nameof(filterContext));

                //check whether this filter has been overridden for the action
                AuthorizeAdminAttribute actionFilter = filterContext.ActionDescriptor.FilterDescriptors
                    .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<AuthorizeAdminAttribute>().FirstOrDefault();

                //ignore filter (the action is available even if a customer hasn't access to the admin area)
                if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                    return;
                //there is AdminAuthorizeFilter, so check access
                if (filterContext.Filters.Any(filter => filter is AuthorizeAdminFilter))
                {
                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    {
                        var claims = _httpContextAccessor.HttpContext.User.HasClaim(x=>x.Value==CustomerRole.Constants.AccessAdminPanel);
                        //authorize permission of access to the admin area
                        if (!claims)
                         filterContext.Result = new ChallengeResult();
                    }
                    else
                    {
                        filterContext.Result = new ChallengeResult();
                    }

                }
            }

            #endregion
        }

        #endregion
    }
}