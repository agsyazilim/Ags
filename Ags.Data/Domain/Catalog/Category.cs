using System;
using Ags.Data.Common;
using Ags.Data.Domain.Seo;

namespace Ags.Data.Domain.Catalog
{
    /// <summary>
    /// Class Category.
    /// </summary>
    
    public partial class Category :BaseEntity, ISlugSupported
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
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
        /// Gets or sets the parent category identifier.
        /// </summary>
        /// <value>The parent category identifier.</value>
        public int ParentCategoryId { get; set; }
        /// <summary>
        /// Gets or sets the picture identifier.
        /// </summary>
        /// <value>The picture identifier.</value>
        public int PhotoGalleryId { get; set; }

        public int VideoGalleryId { get; set; }

        public int SliderId { get; set; }
        /// <summary>
        /// BannerPictureId
        /// </summary>
        public int BannerPictureId { get; set; }
        /// <summary>
        /// BannerLitlePictureId
        /// </summary>
        public int BannerLitlePictureId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show on home page].
        /// </summary>
        /// <value><c>true</c> if [show on home page]; otherwise, <c>false</c>.</value>
        public bool ShowOnHomePage { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [include in top menu].
        /// </summary>
        /// <value><c>true</c> if [include in top menu]; otherwise, <c>false</c>.</value>
        public bool IncludeInTopMenu { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [include in footer menu].
        /// </summary>
        /// <value><c>true</c> if [include in footer menu]; otherwise, <c>false</c>.</value>
        public bool IncludeInFooterMenu { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [include in manset].
        /// </summary>
        /// <value><c>true</c> if [include in manset]; otherwise, <c>false</c>.</value>
        public bool IncludeInManset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [limited to stores].
        /// </summary>
        /// <value><c>true</c> if [limited to stores]; otherwise, <c>false</c>.</value>
        public bool LimitedToStores { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Category" /> is published.
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Category" /> is deleted.
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// Gets or sets the category template identifier.
        /// </summary>
        /// <value>The category template identifier.</value>
        public int CategoryTemplateId { get; set; }
        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the available customer selectable page size options
        /// </summary>
        /// <value>The page size options.</value>
        public string PageSizeOptions { get; set; }
        /// <summary>
        /// Gets or sets the created on UTC.
        /// </summary>
        /// <value>The created on UTC.</value>
        public DateTime CreatedOnUtc { get; set; }
        /// <summary>
        /// Gets or sets the updated on UTC.
        /// </summary>
        /// <value>The updated on UTC.</value>
        public DateTime UpdatedOnUtc { get; set; }


    }
}
