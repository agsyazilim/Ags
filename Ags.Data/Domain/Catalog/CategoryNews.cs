using Ags.Data.Common;
using Ags.Data.Domain.News;

namespace Ags.Data.Domain.Catalog
{
    /// <summary>
    /// Class CategoryNews.
    /// Implements the <see cref="BaseEntity" />
    /// Implements the <see cref="Nop.Core.Domain.Security.IAclSupported" />
    /// </summary>
    /// <seealso cref="BaseEntity" />
    /// <seealso cref="Nop.Core.Domain.Security.IAclSupported" />
    public partial class CategoryNews :BaseEntity 
    {
        /// <summary>
        /// Creates new sid.
        /// </summary>
        /// <value>The news identifier.</value>
        public int NewsId { get; set; }
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>The category identifier.</value>
        public int CategoryId { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public virtual Category Category { get; set; }
        /// <summary>
        /// Creates new s.
        /// </summary>
        /// <value>The news.</value>
        public virtual NewsItem News { get; set; }
       
    }
}
