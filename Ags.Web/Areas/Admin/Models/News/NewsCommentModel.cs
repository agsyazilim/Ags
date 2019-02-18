using System;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news comment model
    /// </summary>
    public partial class NewsCommentModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("Haber Id :")]
        public int NewsItemId { get; set; }

        [AgsDisplayName("Haber Başlığı :")]
        public string NewsItemTitle { get; set; }

        [AgsDisplayName("KullanıcıId :")]
        public int CustomerId { get; set; }

        [AgsDisplayName("Kullanıcı :")]
        public string CustomerInfo { get; set; }

        [AgsDisplayName("Yorum Başlığı :")]
        public string CommentTitle { get; set; }

        [AgsDisplayName("Yorum İçerik :")]
        public string CommentText { get; set; }

        [AgsDisplayName("Onaylı mı? :")]
        [UIHint("Boolean")]
        public bool IsApproved { get; set; }

        [AgsDisplayName("Site :")]
        public int StoreId { get; set; }

        public string StoreName { get; set; }

        [AgsDisplayName("Oluşturma Tarihi :")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}