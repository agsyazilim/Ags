using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a category search model
    /// </summary>
    public partial class CategorySearchModel : BaseSearchModel
    {
        #region Properties
        /// <summary>
        /// Kategoriye Göre Arama
        /// </summary>
        [Display(Name = "Kategori Adına Göre Ara")]
        public string SearchCategoryName { get; set; }
        #endregion
    }
}