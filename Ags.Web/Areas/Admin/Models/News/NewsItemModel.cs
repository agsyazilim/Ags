using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Areas.Admin.Models.Media.Galery;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news item model
    /// </summary>
    public partial class NewsItemModel : BaseAgsEntityModel
    {
        public NewsItemModel()
        {
            AvailableEditors = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            SelectedCategoryIds = new List<int>();
            AddGaleryPictureModel = new AddGaleryPictureModel();
            AddGaleryPictureModels = new List<AddGaleryPictureModel>();
            GalleryPictureSearchModel = new GalleryPictureSearchModel();

        }

        #region Properties

        [AgsDisplayName("Haber Başlığı :")]
        [Required]
        public string Title { get; set; }

        [AgsDisplayName("Kısa Açıklama :")]
        [Required]
        public string Short { get; set; }

        [AgsDisplayName("Haber İçeriği :")]
        [Required]
        public string Full { get; set; }

        [AgsDisplayName("Herkes Yorum Yazabilir :")]
        public bool AllowComments { get; set; }
        [AgsDisplayName("Başlık Çıkmasın :")]
        [UIHint("Boolean")]
        public bool NoTitle { get; set; }

        [UIHint("Picture")]
        [AgsDisplayName("Haber için Resim :")]
        public int PictureId { get; set; }
        [AgsDisplayName("Video :")]
        public int VideoId { get; set; }
        [AgsDisplayName("Video Yerleştirme Kodu :")]
        public string VideoEmbedCode { get; set; }
        [AgsDisplayName("AnaSayfa Büyük Slider :")]
        [UIHint("Boolean")]
        public bool MainBanner { get; set; }
        [AgsDisplayName("Son Haberlerde Çıksın :")]
        [UIHint("Boolean")]
        public bool LastNews { get; set; }
        [AgsDisplayName("Anasayfa Küçük Slider :")]
        [UIHint("Boolean")]
        public bool SecondBanner { get; set; }
        [AgsDisplayName("Slider altında Banner da Çıksın :")]
        [UIHint("Boolean")]
        public bool UpBanner { get; set; }
        [AgsDisplayName("Flaş Haber :")]
        [UIHint("Boolean")]
        public bool FlashNews { get; set; }
        [AgsDisplayName("Haber Onaylı :")]
        [UIHint("Boolean")]
        public bool Approve { get; set; }
        [AgsDisplayName("Yanınlanma Tarihi :")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [AgsDisplayName("Yayından Kaldırma Tarihi :")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }

        [AgsDisplayName("MetaKeywords :")]
        public string MetaKeywords { get; set; }

        [AgsDisplayName("MetaDescription :")]
        public string MetaDescription { get; set; }

        [AgsDisplayName("MetaTitle :")]
        public string MetaTitle { get; set; }

        [AgsDisplayName("SeName :")]
        public string SeName { get; set; }

        [AgsDisplayName("Yayında :")]
        [UIHint("Boolean")]
        public bool Published { get; set; }
        [AgsDisplayName("Tab Büyük Resim Olarak Gelsin :")]
        [UIHint("Boolean")]
        public bool BigNews { get; set; }

        public int ApprovedComments { get; set; }

        public int NotApprovedComments { get; set; }

        [AgsDisplayName("Oluşturma Tarihi :")]
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// editorid
        /// </summary>
        [AgsDisplayName("Editor :")]
        public int CustomerId { get; set; }
        [AgsDisplayName("Editör Seçin :")]
        public IList<SelectListItem> AvailableEditors { get; set; }
        //categories
        [AgsDisplayName("Kategori Seçin :")]
        [Required]
        public IList<int> SelectedCategoryIds { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
        public AddGaleryPictureModel AddGaleryPictureModel { get; set; }
        public IList<AddGaleryPictureModel> AddGaleryPictureModels { get; set; }
        public GalleryPictureSearchModel GalleryPictureSearchModel { get; set; }


        #endregion
    }
}