using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news comment search model
    /// </summary>
    public partial class NewsCommentSearchModel : BaseSearchModel
    {
        #region Ctor

        public NewsCommentSearchModel()
        {
            AvailableApprovedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AgsDisplayName("Habere Göre Ara")]
        [UIHint("Int32Nullable")]
        public int? NewsItemId { get; set; }

        [AgsDisplayName("Oluşturma Tarihi")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [AgsDisplayName("Gönderilme Tarihi")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [AgsDisplayName("Aranan Kelime")]
        public string SearchText { get; set; }

        [AgsDisplayName("Onayla Göre Ara")]
        public int SearchApprovedId { get; set; }

        public IList<SelectListItem> AvailableApprovedOptions { get; set; }

        #endregion
    }
}