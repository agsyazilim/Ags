using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Enews
{
    /// <summary>
    /// Represents a news item model
    /// </summary>
    public partial class ENewsItemModel : BaseAgsEntityModel
    {
        public ENewsItemModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }
        #region Properties
        [AgsDisplayName("Egazete")]
        public string Name { get; set; }
        [UIHint("Picture")]
        [AgsDisplayName("Resim")]
        public int PictureId { get; set; }
        public string SeName { get; set; }
        [AgsDisplayName("Oluşturma Tarihi")]
        public DateTime CreatedOn { get; set; }
        [AgsDisplayName("Kategori Adı")]
        public string Categori { get; set; }
        //categories
        [AgsDisplayName("Kategori Seçin")]
        public int SelectedCategoryId { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }


        #endregion
    }
}