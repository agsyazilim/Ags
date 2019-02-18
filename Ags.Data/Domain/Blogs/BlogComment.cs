using System;
using Ags.Data.Common;
using Ags.Data.Domain.Customers;

namespace Ags.Data.Domain.Blogs
{
    /// <summary>
    /// Class BlogComment.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    /// <summary>
    /// Class BlogComment.
    /// Implements the <see cref="BaseEntity" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    public partial class BlogComment:BaseEntity
    {

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        public string CommentText { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value><c>true</c> if this instance is approved; otherwise, <c>false</c>.</value>
        /// <summary>
        /// Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value><c>true</c> if this instance is approved; otherwise, <c>false</c>.</value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the blog post identifier.
        /// </summary>
        /// <value>The blog post identifier.</value>
        public int BlogPostId { get; set; }
        /// <summary>
        /// Gets or sets the created on UTC.
        /// </summary>
        /// <value>The created on UTC.</value>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the blog post.
        /// </summary>
        /// <value>The blog post.</value>
        public virtual BlogPost BlogPost { get; set; }

        public virtual Customer Customer { get; set; }


    }
}
