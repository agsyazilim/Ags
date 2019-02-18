// ***********************************************************************
// Assembly         : Ags.Web
// Author           : kayaa
// Created          : 12-28-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="Customer.cs" company="Ags.Web">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Ags.Data.Common;

namespace Ags.Data.Domain.Customers
{
    /// <summary>
    /// Class Customer.
    /// </summary>
    public class Customer :BaseEntity
    {


        // user ID from AspNetUser table.
        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>The owner identifier.</value>
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }
        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        public string Zip { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ContactStatus Status { get; set; }

        public static Customer CreateCustomerEvent(ContactStatus status, string firstName,
            string ownerId, string email,string zip)
        {
            return new Customer{OwnerId=ownerId,Status=ContactStatus.Rejected,Email = email,Name = firstName,Zip = zip};
        }
    }
    /// <summary>
    /// Enum ContactStatus
    /// </summary>
    public enum ContactStatus
    {
        /// <summary>
        /// The submitted
        /// </summary>
        Submitted,
        /// <summary>
        /// The approved
        /// </summary>
        Approved,
        /// <summary>
        /// The rejected
        /// </summary>
        Rejected
    }
}