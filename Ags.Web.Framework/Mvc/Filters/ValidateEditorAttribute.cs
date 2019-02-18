using System;
using System.Linq;
using Ags.Web.Framework.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ags.Web.Framework.Mvc.Filters
{
    /// <summary>
    /// Represents a filter attribute confirming that user with "Vendor" customer role has appropriate vendor account associated (and active)
    /// </summary>
    public class ValidateEditorAttribute : TypeFilterAttribute
    {
        #region Fields

        private readonly bool _ignoreFilter;

        #endregion

        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        /// <param name="ignore">Whether to ignore the execution of filter actions</param>
        public ValidateEditorAttribute(bool ignore = false) : base(typeof(ValidateVendorFilter))
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
        /// Represents a filter confirming that user with "Vendor" customer role has appropriate vendor account associated (and active)
        /// </summary>
        private class ValidateVendorFilter : IAuthorizationFilter
        {
            #region Fields

            private readonly bool _ignoreFilter;
            private readonly IHttpContextAccessor _httpContextAccessor;
            
            #endregion

            #region Ctor

            public ValidateVendorFilter(bool ignoreFilter, IHttpContextAccessor httpContextAccessor)
            {
                this._ignoreFilter = ignoreFilter;
                this._httpContextAccessor = httpContextAccessor;
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

                //check whether this filter has been overridden for the Action
                var actionFilter = filterContext.ActionDescriptor.FilterDescriptors
                    .Where(filterDescriptor => filterDescriptor.Scope == FilterScope.Action)
                    .Select(filterDescriptor => filterDescriptor.Filter).OfType<ValidateEditorAttribute>().FirstOrDefault();

                //ignore filter (the action is available even if the current customer isn't a vendor)
                if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                    return;

               
                //whether current customer is vendor
                if (!_httpContextAccessor.HttpContext.User.IsInRole(CustomerRole.Constants.CustomerManagersRole))
                    return;

                //ensure that this user has active vendor record associated
                if (_httpContextAccessor.HttpContext.User == null)
                    filterContext.Result = new ChallengeResult();
            }

            #endregion
        }

        #endregion
    }
}