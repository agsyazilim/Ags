using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news item search model
    /// </summary>
    public partial class NewsItemSearchModel : BaseSearchModel
    {
        public NewsItemSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableEditor = new List<SelectListItem>();
        }
        [AgsDisplayName("Haber Başlığına Göre Ara")]
        public string SearchNewsTitle { get; set; }
        [AgsDisplayName("Kategoriye Göre Ara")]
        public int SearchCategoryId { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
        [Display(Name = "Editor'e Göre Ara")]
        public int SearchCustomerId { get; set; }

        public IList<SelectListItem> AvailableEditor { get; set; }

        [AgsDisplayName("Başlangıç Tarihi")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [AgsDisplayName("Bitiş Tarihi")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }
        [AgsDisplayName("Yayınlanma Tarihi")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

    }
}