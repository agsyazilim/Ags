// ***********************************************************************
// Assembly         : Ags.Web
// Author           : kayaa
// Created          : 12-28-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="CustomerIsOwnerAuthorizationHandler.cs" company="Ags.Web">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Threading.Tasks;
using Ags.Data.Domain;
using Ags.Data.Domain.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Ags.Web.Framework.Authorization
{
    /// <summary>
    /// Class CustomerIsOwnerAuthorizationHandler.
    /// Implements the <see cref="Customer" />
    /// </summary>
    /// <seealso cref="Customer" />
    public class CustomerIsOwnerAuthorizationHandler
    :AuthorizationHandler<OperationAuthorizationRequirement,Customer>
    {
        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerIsOwnerAuthorizationHandler"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public CustomerIsOwnerAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement and resource.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <param name="resource">The resource to evaluate.</param>
        /// <returns>Task.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,Customer resource)
        {
            if (context.User == null || resource == null)
            {
                // Return Task.FromResult(0) if targeting a version of
                // .NET Framework older than 4.6:
                return Task.CompletedTask;
            }

            // If we're not asking for CRUD permission, return.

            if (requirement.Name != CustomerRole.Constants.CreateOperationName &&
                requirement.Name != CustomerRole.Constants.ReadOperationName &&
                requirement.Name != CustomerRole.Constants.UpdateOperationName &&
                requirement.Name != CustomerRole.Constants.DeleteOperationName&&
                requirement.Name!=CustomerRole.Constants.AccessAdminPanel)
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerId == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;


        }
    }
}