using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Enews
{
    /// <summary>
    /// Represents a news item search model
    /// </summary>
    public partial class ENewsItemSearchModel : BaseSearchModel
    {
        public ENewsItemSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }

        [AgsDisplayName("Kategoriye Göre Ara")]
        public int SearchCategoryId { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
        [AgsDisplayName("Yayınlanma Tarihi")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

    }
}