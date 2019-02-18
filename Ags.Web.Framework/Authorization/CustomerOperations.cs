// ***********************************************************************
// Assembly         : Ags.Web
// Author           : kayaa
// Created          : 12-28-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="CustomerOperations.cs" company="Ags.Web">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Ags.Web.Framework.Authorization
{
    /// <summary>
    /// Class CustomerOperations.
    /// </summary>
    public static class CustomerOperations
    {
        /// <summary>
        /// The create
        /// </summary>
        public static OperationAuthorizationRequirement Create =
         new OperationAuthorizationRequirement { Name = CustomerRole.Constants.CreateOperationName };
        /// <summary>
        /// The read
        /// </summary>
        public static OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = CustomerRole.Constants.ReadOperationName };
        /// <summary>
        /// The update
        /// </summary>
        public static OperationAuthorizationRequirement Update =
          new OperationAuthorizationRequirement { Name = CustomerRole.Constants.UpdateOperationName };
        /// <summary>
        /// The delete
        /// </summary>
        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = CustomerRole.Constants.DeleteOperationName };
        /// <summary>
        /// The approve
        /// </summary>
        public static OperationAuthorizationRequirement Approve =
          new OperationAuthorizationRequirement { Name = CustomerRole.Constants.ApproveOperationName };
        /// <summary>
        /// The reject
        /// </summary>
        public static OperationAuthorizationRequirement Reject =
          new OperationAuthorizationRequirement { Name = CustomerRole.Constants.RejectOperationName };

        public static OperationAuthorizationRequirement AccessAdminPanel = new OperationAuthorizationRequirement
        {
            Name = CustomerRole.Constants.AccessAdminPanel
        };

        public static OperationAuthorizationRequirement PublicSite = new OperationAuthorizationRequirement{Name = CustomerRole.Constants.PublicAccessSite};
    }

}