using System;
using System.Linq;
using Ags.Data.Domain;
using Ags.Services.Customers;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Ags.Web.Factories
{
    /// <summary>
    /// Represents the common models factory
    /// </summary>
    public partial class CommonModelFactory : ICommonModelFactory
    {
        #region Fields


        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ICustomerService _customerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;



        #endregion

        #region Ctor

        public CommonModelFactory(
            IActionContextAccessor actionContextAccessor,
            ICustomerService customerService,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this._actionContextAccessor = actionContextAccessor;
            this._customerService = customerService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion



        #region Methods

        /// <summary>
        /// Prepare the contact us model
        /// </summary>
        /// <param name="model">Contact us model</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <returns>Contact us model</returns>
        public virtual ContactUsModel PrepareContactUsModel(ContactUsModel model, bool excludeProperties)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!excludeProperties)
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    System.Security.Claims.ClaimsPrincipal userClaimsPrincipal = _httpContextAccessor.HttpContext.User;
                    ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.UserName == userClaimsPrincipal.Identity.Name);
                    if (user != null)
                    {
                        model.Email = user.Email;
                        model.FullName = user.FirstName + user.LastName;
                    }
                }

            }
            model.SubjectEnabled = true;


            return model;
        }


        #endregion
    }
}