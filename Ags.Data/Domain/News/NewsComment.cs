// ***********************************************************************
// Assembly         : Nop.Core
// Author           : kayaa
// Created          : 12-26-2018
//
// Last Modified By : kayaa
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="NewsComment.cs" company="Nop Solutions, Ltd">
//     Copyright © Nop Solutions, Ltd
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Ags.Data.Common;
using Ags.Data.Domain.Customers;

namespace Ags.Data.Domain.News
{
    /// <summary>
    /// Class NewsComment.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class NewsComment :BaseEntity
    {
        /// <summary>
        /// Gets or sets the comment title.
        /// </summary>
        /// <value>The comment title.</value>
       
        public string CommentTitle { get; set; }
        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        public string CommentText { get; set; }
        /// <summary>
        /// Creates new sitemid.
        /// </summary>
        /// <value>The news item identifier.</value>
        public int NewsItemId { get; set; }
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value><c>true</c> if this instance is approved; otherwise, <c>false</c>.</value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the created on UTC.
        /// </summary>
        /// <value>The created on UTC.</value>
        public DateTime CreatedOnUtc { get; set; }

        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the news item
        /// </summary>
        /// <value>The news item.</value>
        public virtual NewsItem NewsItem { get; set; }


    }
}
