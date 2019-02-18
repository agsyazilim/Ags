using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Topics
{
    /// <summary>
    /// Represents a topic model
    /// </summary>
    public partial class TopicModel : BaseAgsEntityModel
    {
        #region Ctor

        public TopicModel()
        {
            AvailableTopicTemplates = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AgsDisplayName("Sistem Adı :")]
        [Required]
        public string SystemName { get; set; }

        [AgsDisplayName("Site Mape Ekle :")]
        [UIHint("Boolean")]
        public bool IncludeInSitemap { get; set; }

        [AgsDisplayName("Üst Menüye Ekle :")]
        [UIHint("Boolean")]
        public bool IncludeInTopMenu { get; set; }

        [AgsDisplayName("Alt Menü de 1 Kolona Ekle :")]
        [UIHint("Boolean")]
        public bool IncludeInFooterColumn1 { get; set; }

        [AgsDisplayName("Alt Kolona 2 ye Ekle :")]
        [UIHint("Boolean")]
        public bool IncludeInFooterColumn2 { get; set; }

        [AgsDisplayName("Alt Menü 3 Ekle :")]
        [UIHint("Boolean")]
        public bool IncludeInFooterColumn3 { get; set; }

        [AgsDisplayName("Sırası :")]
        public int DisplayOrder { get; set; }

        [AgsDisplayName("Site Kapalı İken Görünsün :")]
        [UIHint("Boolean")]
        public bool AccessibleWhenStoreClosed { get; set; }

        [AgsDisplayName("IsPasswordProtected :")]
        public bool IsPasswordProtected { get; set; }

        [AgsDisplayName("Şifre :")]
        public string Password { get; set; }

        [AgsDisplayName("Url :")]
        public string Url { get; set; }

        [AgsDisplayName("Başlık :")]
        
        public string Title { get; set; }

        [AgsDisplayName("Yazı :")]
        public string Body { get; set; }

        [AgsDisplayName("Aktif :")]
        [UIHint("Boolean")]
        public bool Published { get; set; }

        [AgsDisplayName("Şablonu :")]
        [Required]
        public int TopicTemplateId { get; set; }

        public IList<SelectListItem> AvailableTopicTemplates { get; set; }

        [AgsDisplayName("Meta Keywords :")]
        public string MetaKeywords { get; set; }

        [AgsDisplayName("Meta Description :")]
        public string MetaDescription { get; set; }

        [AgsDisplayName("Meta Title :")]
        public string MetaTitle { get; set; }

        [AgsDisplayName("Seo Name :")]
        public string SeName { get; set; }

        #endregion
    }


}