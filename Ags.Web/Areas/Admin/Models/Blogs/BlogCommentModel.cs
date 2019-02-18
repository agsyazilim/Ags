using System;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Blogs
{
    /// <summary>
    /// Represents a blog comment model
    /// </summary>
    public partial class BlogCommentModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("Id :")]
        public int BlogPostId { get; set; }

        [AgsDisplayName("Başlık :")]
        public string BlogPostTitle { get; set; }

        [AgsDisplayName("Kullanıcı :")]
        public int CustomerId { get; set; }

        [AgsDisplayName("Kullanıcı Bilgisi :")]
        public string CustomerInfo { get; set; }

        [AgsDisplayName("Yorum :")]
        public string Comment { get; set; }

        [AgsDisplayName("Onaylı :")]
        [UIHint("Boolean")]
        public bool IsApproved { get; set; }

        [AgsDisplayName("Oluşturma :")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}