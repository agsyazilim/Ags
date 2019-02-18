using System;
using System.Collections.Generic;
using Ags.Data.Common;
using Ags.Data.Domain.Customers;
using Ags.Data.Domain.Seo;

namespace Ags.Data.Domain.Blogs
{
    
    public partial class BlogPost :BaseEntity , ISlugSupported 
    {

        /// <summary>
        /// The blog comments
        /// </summary>
        private ICollection<BlogComment> _blogComments;
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        public int CustomerId { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets the body overview.
        /// </summary>
        /// <value>The body overview.</value>
        public string BodyOverview { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [allow comments].
        /// </summary>
        /// <value><c>true</c> if [allow comments]; otherwise, <c>false</c>.</value>
        public bool AllowComments { get; set; }
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public string Tags { get; set; }
        /// <summary>
        /// Gets or sets the start date UTC.
        /// </summary>
        /// <value>The start date UTC.</value>
        public DateTime? StartDateUtc { get; set; }
        /// <summary>
        /// Gets or sets the end date UTC.
        /// </summary>
        /// <value>The end date UTC.</value>
        public DateTime? EndDateUtc { get; set; }
        /// <summary>
        /// Gets or sets the meta keywords.
        /// </summary>
        /// <value>The meta keywords.</value>
        public string MetaKeywords { get; set; }
        /// <summary>
        /// Gets or sets the meta description.
        /// </summary>
        /// <value>The meta description.</value>
        public string MetaDescription { get; set; }
        /// <summary>
        /// Gets or sets the meta title.
        /// </summary>
        /// <value>The meta title.</value>
        public string MetaTitle { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [limited to stores].
        /// </summary>
        /// <value><c>true</c> if [limited to stores]; otherwise, <c>false</c>.</value>
        public bool LimitedToStores { get; set; }
        /// <summary>
        /// Gets or sets the created on UTC.
        /// </summary>
        /// <value>The created on UTC.</value>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// Gets or sets the blog comments.
        /// </summary>
        /// <value>The blog comments.</value>
        public virtual ICollection<BlogComment> BlogComments
        {
            get=>_blogComments??(_blogComments=new List<BlogComment>());
            protected set=>_blogComments=value; }

    
    }
}
