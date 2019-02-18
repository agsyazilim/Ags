using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Blogs
{
    /// <summary>
    /// Represents a blog post model
    /// </summary>
    
    public partial class BlogPostModel : BaseAgsEntityModel
    {
        public BlogPostModel()
        {
            AvailableEditors = new List<SelectListItem>();
        }
        #region Properties

        [Required(ErrorMessage = "Başlık Girmelisiniz")]
        [MinLength(5,ErrorMessage = "En az 5 Karakter Girmelisiniz ")]
        [MaxLength(50,ErrorMessage = "En Fazla 50")]
        [AgsDisplayName("Başlık :")]
        public string Title { get; set; }
        [Required(ErrorMessage = "İçerik Girmelisiniz")]
        [AgsDisplayName("Yazı :")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Kısa açıklama Giriniz")]
        [MinLength(10,ErrorMessage = "En Az 10 Karakter Giriniz")]
        [AgsDisplayName("Kısa Açıklama :")]
        public string BodyOverview { get; set; }

        [AgsDisplayName("Herkes Yorum Yazabilir :")]
        [UIHint("Boolean")]
        public bool AllowComments { get; set; }

        [AgsDisplayName("Etiket :")]
        public string Tags { get; set; }

        public int ApprovedComments { get; set; }

        public int NotApprovedComments { get; set; }

        [AgsDisplayName("Yayınlama Tarihi :")]
        [UIHint("DateTimeNullable")]
        [Required(ErrorMessage = "Başlangıç Zamanı Seçiniz")]
        public DateTime? StartDate { get; set; }

        [AgsDisplayName("Yayından Kaldırma :")]
        [Required(ErrorMessage = "Yayından Kalma Zamanını Seçiniz")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }

        [AgsDisplayName("Meta Keywords :")]
        public string MetaKeywords { get; set; }

        [AgsDisplayName("Meta Description :")]
        public string MetaDescription { get; set; }

        [AgsDisplayName("Meta Title :")]
        public string MetaTitle { get; set; }

        [AgsDisplayName("SeName :")]
        public string SeName { get; set; }

        [AgsDisplayName("Oluşturma Tarihi :")]
        public DateTime CreatedOn { get; set; }
        [Required(ErrorMessage = "Lütfen Editör Seçiniz")]
        [Range(1,100000,ErrorMessage = "Editor Seçiniz")]
        [AgsDisplayName("Editör :")]
        public int CustomerId { get; set; }
        public IList<SelectListItem> AvailableEditors { get; set; }


        #endregion
    }
}