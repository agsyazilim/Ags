using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ags.Data.Core.Caching;
using Ags.Data.Domain.Catalog;

namespace Ags.Services.Catalog
{
    /// <summary>
    /// Category (for caching)
    /// </summary>
    [Serializable]
    //Entity Framework will assume that any class that inherits from a POCO class that is mapped to a table on the database requires a Discriminator column
    //That's why we have to add [NotMapped] as an attribute of the derived class.
    [NotMapped]
    public class CategoryForCaching : Category, IEntityForCaching
    {
        public CategoryForCaching()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="c">Category to copy</param>
        public CategoryForCaching(Category c)
        {
            Id = c.Id;
            Name = c.Name;
            Description = c.Description;
            CategoryTemplateId = c.CategoryTemplateId;
            MetaKeywords = c.MetaKeywords;
            MetaDescription = c.MetaDescription;
            MetaTitle = c.MetaTitle;
            ParentCategoryId = c.ParentCategoryId;
            PhotoGalleryId = c.PhotoGalleryId;
            VideoGalleryId = c.VideoGalleryId;
            SliderId = c.SliderId;
            PageSize = c.PageSize;
            PageSizeOptions = c.PageSizeOptions;
            ShowOnHomePage = c.ShowOnHomePage;
            IncludeInTopMenu = c.IncludeInTopMenu;
            IncludeInFooterMenu = c.IncludeInFooterMenu;
            IncludeInManset = c.IncludeInManset;
            LimitedToStores = c.LimitedToStores;
            Published = c.Published;
            Deleted = c.Deleted;
            DisplayOrder = c.DisplayOrder;
            CreatedOnUtc = c.CreatedOnUtc;
            UpdatedOnUtc = c.UpdatedOnUtc;
            BannerLitlePictureId = c.BannerLitlePictureId;
            BannerPictureId = c.BannerPictureId;
        }


    }
}