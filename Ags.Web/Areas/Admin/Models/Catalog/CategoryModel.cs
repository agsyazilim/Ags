using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a category model
    /// </summary>
    public partial class CategoryModel : BaseAgsEntityModel
    {
        #region Ctor

        public CategoryModel()
        {
            if (PageSize < 1)
            {
                PageSize = 15;
            }

            AvailableCategoryTemplates = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            AvailableVideoGalleries = new List<SelectListItem>();
            AvailablePhotoGallery = new List<SelectListItem>();
            AvailableSliders = new List<SelectListItem>();

        }

        #endregion

        #region Properties
        [Required(ErrorMessage = "Kategori Adı Giriniz")]
        [AgsDisplayName("Kategori Adı :")]
        [MinLength(3)]
        public string Name { get; set; }
        [AgsDisplayName("Açıklama :")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Temlate Seçiniz")]
        [AgsDisplayName("Şablon :")]
        public int CategoryTemplateId { get; set; }
        public IList<SelectListItem> AvailableCategoryTemplates { get; set; }
        [Display(Name = "Anahtar Kelime :")]
        public string MetaKeywords { get; set; }
        [AgsDisplayName("Meta Açıklama :")]
        public string MetaDescription { get; set; }
        [AgsDisplayName("Meta Title :")]
        public string MetaTitle { get; set; }
        [AgsDisplayName("SeName :")]
        public string SeName { get; set; }
        [AgsDisplayName("Parent :")]
        public int ParentCategoryId { get; set; }
        [AgsDisplayName("Sayfada Çıkacak Haber Sayısı :")]
        public int PageSize { get; set; }
        [AgsDisplayName("Sayfa Opsiyonu :")]
        public string PageSizeOptions { get; set; }
        [AgsDisplayName("Anasayfada Göster :")]
        [UIHint("Boolean")]
        public bool ShowOnHomePage { get; set; }
        [AgsDisplayName("Üst Menüye Ekle :")]
        [UIHint("Boolean")]
        public bool IncludeInTopMenu { get; set; }
        [AgsDisplayName("Alt Menüye Ekle :")]
        [UIHint("Boolean")]
        public bool IncludeInFooterMenu { get; set; }
        [AgsDisplayName("Manşet Ekle :")]
        [UIHint("Boolean")]
        public bool IncludeInManset { get; set; }

        [AgsDisplayName("Yayında :")]
        [UIHint("Boolean")]
        public bool Published { get; set; }

        [AgsDisplayName("Silindi :")]
        [UIHint("Boolean")]
        public bool Deleted { get; set; }

        /// <summary>
        /// BannerPictureId
        /// </summary>
        [UIHint("Picture")]
        [Display(Name = "Banner Resim :")]
        public int BannerPictureId { get; set; }
        /// <summary>
        /// BannerLitlePictureId
        /// </summary>
        [UIHint("Picture")]
        [Display(Name = "Banner Kücük Resim :")]
        public int BannerLitlePictureId { get; set; }

        [AgsDisplayName("Sıra :")]
        public int DisplayOrder { get; set; }
        [AgsDisplayName("Foto Galleri Seçebilirsiniz :")]
        public int PhotoGalleryId { get; set; }
        [AgsDisplayName("Video Galeri Seçebilirsiniz :")]
        public int VideoGalleryId { get; set; }
        [AgsDisplayName("Slider Seçebilirsiniz :")]
        public int SliderId { get; set; }

        public string Breadcrumb { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailablePhotoGallery { get; set; }
        public IList<SelectListItem> AvailableVideoGalleries { get; set; }
        public IList<SelectListItem> AvailableSliders { get; set; }

        #endregion
    }


}