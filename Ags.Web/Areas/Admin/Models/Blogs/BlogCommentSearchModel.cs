using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Blogs
{
    /// <summary>
    /// Represents a blog comment search model
    /// </summary>
    public partial class BlogCommentSearchModel : BaseSearchModel
    {
        #region Ctor

        public BlogCommentSearchModel()
        {
            AvailableApprovedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AgsDisplayName("Blog Id :")]
        [UIHint("Int32Nullable")]
        public int? BlogPostId { get; set; }

        [AgsDisplayName("Oluşturma Tarihi :")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [AgsDisplayName("Yayınlanma Tarihi :")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [AgsDisplayName("Arama için kelime Giriniz :")]
        public string SearchText { get; set; }

        [AgsDisplayName("Sadece Onaylılar :")]
        public int SearchApprovedId { get; set; }

        public IList<SelectListItem> AvailableApprovedOptions { get; set; }

        #endregion
    }
}